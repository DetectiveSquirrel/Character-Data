using CharacterData.ExperienceTable;
using CharacterData.Utils;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DeployedObject = ExileCore.PoEMemory.Components.DeployedObject;
using Vector2 = System.Numerics.Vector2;

namespace CharacterData
{
    public class CharacterData : BaseSettingsPlugin<CharacterDataSettings>
    {
        public static CharacterData MainPlugin { get; set; }

        public List<Entity> PlayerEntities { get; set; } = new List<Entity>();

        public static List<PartyElementWindow> PlayerInPartyDraw { get; set; } = new List<PartyElementWindow>();

        public static string BaseConfigDirectory { get; set; }

        public static InstanceSnapshot Instance { get; set; }

        public static LogRun Run { get; set; }

        public int Kills { get; private set; }

        public List<Buff> buffs;

        private static bool ShouldLog => LocalPlayer.Experience != Instance.JoinExperience || LocalPlayer.Kills != Instance.JoinKills;

        public static int TryGetStat(GameStat stat, Entity entity)
        {
            if (entity.TryGetComponent<Stats>(out var statsComp))
            {
                if (statsComp.StatDictionary.TryGetValue(stat, out var statValue))
                {
                    return statValue;
                }
            }

            return 0;
        }

        public int TryGetStat(GameStat stat)
        {
            if (GameController.Player.TryGetComponent<Stats>(out var statsComp))
            {
                if (statsComp.StatDictionary.TryGetValue(stat, out var statValue))
                {
                    return statValue;
                }
            }

            return 0;
        }

        public override void AreaChange(AreaInstance area)
        {
            ResetData();
        }

        private void ResetData()
        {
            LogRun();

            Instance = new InstanceSnapshot();
            Run = new LogRun();
        }

        public override bool Initialise()
        {
            BaseConfigDirectory = ConfigDirectory;
            MainPlugin = this;
            Kills = TryGetStat(GameStat.CharacterKillCount);
            Instance = new InstanceSnapshot();
            Run = new LogRun();
            return true;
        }

        public override void DrawSettings()
        {
            if (ImGui.TreeNode("Player Kill Leaderboard"))
            {
                var entities = GameController.EntityListWrapper.ValidEntitiesByType[EntityType.Player];
                var nameToKillsList = new Dictionary<string, int>();

                foreach (var entity in entities)
                {
                    var playerComp = entity.GetComponent<Player>();
                    if (playerComp.PlayerName != LocalPlayer.Name)
                    {
                        nameToKillsList.Add($"{playerComp.PlayerName,-23} ({playerComp.Level})", TryGetStat(GameStat.CharacterKillCount, entity));
                    }
                }

                var sortedDict = from entry in nameToKillsList orderby entry.Value descending select entry;

                foreach (var item in sortedDict)
                {
                    ImGui.BulletText($"{item.Key}: {string.Format("{0:#,##0}", item.Value)}");
                }

                ImGui.TreePop();
            }

            ImGui.Separator();

            base.DrawSettings();
        }

        public override void Render()
        {
            buffs = LocalPlayer.Entity.GetComponent<Buffs>().BuffsList;

            Background();
            GlobePercents();
            DrawExperienceData();
            DrawResistances();
            DrawDelveInfo();
            DrawExperiencepercentbar();
            DrawDeployedActorObjects();

            Kills = TryGetStat(GameStat.CharacterKillCount);

            if (Settings.SoulgainPrev)
            {
                // VLS Soul Gain Prevention Timer, you know....for fuck ups.
                var buffOut = buffs.FirstOrDefault(buff => buff.Name == "cannot_gain_souls");
                if (buffOut != null && buffOut.Timer > 0.0)
                {
                    Graphics.DrawText($"Soul Gain Prevention\n{buffOut.Timer:0.##}",
                        new Vector2(Settings.SoulGainPrevX, Settings.SoulGainPrevY),
                        Settings.SoulGainPrevColor);
                }
            }

            if (Settings.Wardloop && !GameController.Area.CurrentArea.IsTown)
            {
                // Wardloop, you know....for fuck ups.
                var wardloopBuffOut = buffs.FirstOrDefault(buff => buff.Name == "flask_bonus_ward_not_break");
                if (wardloopBuffOut == null)
                {
                    Graphics.DrawText($"WARDLOOP BAD",
                        new Vector2(Settings.WardLoopX, Settings.WardLoopY),
                        Settings.WardloopColor, "ProggyClean:46", FontAlign.Center);
                }
            }

            PlayerInPartyDraw = PartyElements.GetPlayerInfoElementList(GameController);

            foreach (var partyElementWindow in PlayerInPartyDraw)
            {
                DrawPartyElementStats(partyElementWindow);
            }
        }

        private void DrawPartyElementStats(PartyElementWindow partyElementWindow)
        {
            var fontSize = 13;
            if (!ShouldDrawPartyElement(partyElementWindow)) return;

            var statCount = 1;

            var xy = partyElementWindow.Element.GetClientRect().TopRight;
            var partyinfoRec = new RectangleF(xy.X, xy.Y, 0, fontSize * (statCount + 1));
            Graphics.DrawBox(partyinfoRec, new Color(0, 0, 0, 185));

            DrawStatText($"Level: {partyElementWindow.Player?.GetComponent<Player>()?.Level ?? 0} ({Progress(partyElementWindow.Player):N2}%)", Settings.LevelTextColor);
            DrawStatText($"Fire: {TryGetStat(GameStat.FireDamageResistancePct, partyElementWindow.Player)}", Settings.FireResistanceColor);
            DrawStatText($"Cold: {TryGetStat(GameStat.ColdDamageResistancePct, partyElementWindow.Player)}", Settings.ColdResistanceColor);
            DrawStatText($"Light: {TryGetStat(GameStat.LightningDamageResistancePct, partyElementWindow.Player)}", Settings.LightningResistanceColor);
            DrawStatText($"Chaos: {TryGetStat(GameStat.ChaosDamageResistancePct, partyElementWindow.Player)}", Settings.ChaosResistanceColor);
            DrawStatText($"HP: {partyElementWindow.Player?.GetComponent<Life>()?.CurHP ?? 0} / ES {partyElementWindow.Player?.GetComponent<Life>()?.CurES ?? 0}", Color.White);
            DrawStatText($"Kills: {string.Format("{0:#,##0}", TryGetStat(GameStat.CharacterKillCount, partyElementWindow.Player))}", Color.White);

            void DrawStatText(string text, Color color)
            {
                Graphics.DrawText(text, xy.ToVector2Num(), color);
                statCount++;
                xy.Y += fontSize;
            }
        }

        private bool ShouldDrawPartyElement(PartyElementWindow partyElementWindow)
        {
            return partyElementWindow.Player?.IsValid != null
                   && !GameController.Game.IngameState.IngameUi.OpenLeftPanel.IsVisibleLocal
                   && !GameController.IngameState.IngameUi.ChatTitlePanel.IsVisibleLocal
                   && GameController.IngameState.IngameUi.ChatPanel.IndexInParent != 140
                   && !GameController.IngameState.IngameUi.Atlas.IsVisibleLocal
                   && !GameController.IngameState.IngameUi.TreePanel.IsVisibleLocal;
        }

        public static double Progress(Entity entity)
        {
            return entity.GetComponent<Player>().Level != 100
                ? (entity.GetComponent<Player>().XP - (double)PlayerExperience.TotalExperience[entity.GetComponent<Player>().Level]) /
                       PlayerExperience.NextExperience[entity.GetComponent<Player>().Level] * 100.0
                : 0.0;
        }

        private void Background()
        {
            Graphics.DrawBox(new RectangleF
            {
                Left = Settings.ResolutionLeft,
                Top = Settings.ResolutionTop,
                Right = Settings.ResolutionRight,
                Bottom = Settings.ResolutionBottom
            }, Settings.LevelBackColor);
        }

        private void LogRun()
        {
            if (!ShouldLog)
            {
                return;
            }

            while (!GameController.Game.IngameState.Data.LocalPlayer.IsValid)
            {
                Thread.Sleep(25);
            }
            Run.Message(Settings.LastAreaDuration.Value);
            Run.NewEntry();
            Run.Save();
        }

        private void DrawExperienceData()
        {
            if (!(bool)Settings.LevelToggle)
            {
                return;
            }

            Graphics.DrawText(Run.NeatString(), new Vector2(Settings.LevelPositionX, Settings.LevelPositionY), Settings.LevelTextColor.Value);
        }

        private void DrawExperiencepercentbar()
        {
            if (!(bool)Settings.ExperienceBar)
            {
                return;
            }
        }

        private void DrawResistances()
        {
            if (!(bool)Settings.Resistances)
            {
                return;
            }

            var FireRes = TryGetStat(GameStat.FireDamageResistancePct);
            var FireResTotal = TryGetStat(GameStat.UncappedFireDamageResistancePct);
            var MaxFireRes = TryGetStat(GameStat.MaximumFireDamageResistancePct) != 0 ? TryGetStat(GameStat.MaximumFireDamageResistancePct) : 75;

            var ColdRes = TryGetStat(GameStat.ColdDamageResistancePct);
            var ColdResTotal = TryGetStat(GameStat.UncappedColdDamageResistancePct);
            var MaxColdRes = TryGetStat(GameStat.MaximumColdDamageResistancePct) != 0 ? TryGetStat(GameStat.MaximumColdDamageResistancePct) : 75;

            var LightRes = TryGetStat(GameStat.LightningDamageResistancePct);
            var LightResTotal = TryGetStat(GameStat.UncappedLightningDamageResistancePct);
            var MaxLightRes = TryGetStat(GameStat.MaximumLightningDamageResistancePct) != 0 ? TryGetStat(GameStat.MaximumLightningDamageResistancePct) : 75;

            var ChaosRes = TryGetStat(GameStat.ChaosDamageResistancePct);
            var ChaosResTotal = TryGetStat(GameStat.UncappedChaosDamageResistancePct);
            var MaxChaosRes = TryGetStat(GameStat.MaximumChaosDamageResistancePct) != 0 ? TryGetStat(GameStat.MaximumChaosDamageResistancePct) : 75;

            var num1 = 0;
            var num2 = 0;
            var num3 = 0;
            var num4 = 0;
            try
            {
                num1 = ResistanceDifference(FireRes, FireResTotal, MaxFireRes);
                num2 = ResistanceDifference(ColdRes, ColdResTotal, MaxColdRes);
                num3 = ResistanceDifference(LightRes, LightResTotal, MaxLightRes);
                num4 = ResistanceDifference(ChaosRes, ChaosResTotal, MaxChaosRes);
            }
            catch
            {
            }
            Graphics.DrawText($"{"Fire:",-8}{FireRes,-5}({(num1 > 0 ? "+" + num1 : num1.ToString())})", new Vector2(Settings.ResistanceX, Settings.ResistanceY), Settings.FireResistanceColor, FontAlign.Left);
            Graphics.DrawText($"{"Cold:",-8}{ColdRes,-5}({(num2 > 0 ? "+" + num2 : num2.ToString())})", new Vector2(Settings.ResistanceX, Settings.ResistanceY + Settings.ResistanceTextSize), Settings.ColdResistanceColor, FontAlign.Left);
            Graphics.DrawText($"{"Light:",-8}{LightRes,-5}({(num3 > 0 ? "+" + num3 : num3.ToString())})", new Vector2(Settings.ResistanceX, Settings.ResistanceY + (Settings.ResistanceTextSize * 2)), Settings.LightningResistanceColor, FontAlign.Left);
            Graphics.DrawText($"{"Chaos:",-8}{ChaosRes,-5}({(num4 > 0 ? "+" + num4 : num4.ToString())})", new Vector2(Settings.ResistanceX, Settings.ResistanceY + (Settings.ResistanceTextSize * 3)), Settings.ChaosResistanceColor, FontAlign.Left);
            Graphics.DrawText($"{"Kills:",-8}{string.Format("{0:#,##0}", LocalPlayer.Kills)}", new Vector2(Settings.ResistanceX, Settings.ResistanceY + (Settings.ResistanceTextSize * 4)), Settings.KillsColor, FontAlign.Left);
        }

        private void DrawDelveInfo()
        {
            if (!(bool)Settings.Delveinfo)
            {
                return;
            }

            var sulphiteCount = GameController.Game.IngameState.ServerData.CurrentSulphiteAmount;
            var azuriteCount = GameController.Game.IngameState.ServerData.CurrentAzuriteAmount;
            try
            {
                Graphics.DrawText($"{"Sulphite: ",-14}{sulphiteCount:#,##0}", new Vector2(Settings.DelveinfoX, Settings.DelveinfoY), Settings.DelveinfoSulphiteColor);
                Graphics.DrawText($"{"Azurite: ",-14}{azuriteCount:#,##0}", new Vector2(Settings.DelveinfoX, Settings.DelveinfoY + Settings.DelveTextSpacing), Settings.DelveinfoAzuriteColor);
            }
            catch
            {
            }
        }

        private void DrawDeployedActorObjects()
        {
            if (!Settings.DeployedActorObjects.Value)
                return;

            if (!GameController.Player.TryGetComponent<Actor>(out var actorComp) || actorComp == null)
                return;

            var deployedObjects = GetFilteredDeployedObjects(actorComp.DeployedObjects);

            if (deployedObjects == null)
                return;

            var sortedDeployedObjects = deployedObjects.OrderByDescending(obj => obj.Value);
            var loopCount = 0;

            foreach (var deployedObject in sortedDeployedObjects)
            {
                DrawDeployedActorText(deployedObject.Value.ToString(), deployedObject.Key, loopCount++);
            }
        }

        private static SortedDictionary<string, int> GetFilteredDeployedObjects(List<DeployedObject> objectList)
        {
            if (objectList == null)
                return null;

            var actorDeployedObjects = new SortedDictionary<string, int>();

            foreach (var deployedObject in objectList.Where(obj => obj.Entity != null && obj.Entity.Address != 0 && !string.IsNullOrEmpty(obj.Entity.RenderName)))
            {
                if (actorDeployedObjects.ContainsKey(deployedObject.Entity.RenderName))
                {
                    actorDeployedObjects[deployedObject.Entity.RenderName]++;
                }
                else
                {
                    actorDeployedObjects.Add(deployedObject.Entity.RenderName, 1);
                }
            }

            return actorDeployedObjects;
        }

        private void DrawDeployedActorText(string value, string key, int loopCount)
        {
            var textPosition = new Vector2(Settings.ActorObjectX, Settings.ActorObjectY - (Settings.ResistanceTextSize * loopCount));
            Graphics.DrawText($"{value,-2}: {key}", textPosition, Settings.ActorObjectColor, FontAlign.Left);
        }

        private static int ResistanceDifference(object capRes, object uncappedRes, object maxCapRes)
        {
            return (int)uncappedRes <= (int)capRes ? (int)capRes - (int)maxCapRes : (int)uncappedRes - (int)capRes;
        }

        private void GlobePercents()
        {
            DrawGlobePercent(Settings.HealthToggle, LocalPlayer.Health.HPPercentage, GameController.IngameState.IngameUi.GameUI.LifeOrb.GetClientRectCache.Center.TranslateToNum(), Settings.HpBackColor, Settings.HpTextColor);
            DrawGlobePercent(Settings.ManaToggle, LocalPlayer.Health.MPPercentage, GameController.IngameState.IngameUi.GameUI.ManaOrb.GetClientRectCache.Center.TranslateToNum(), Settings.MpBackColor, Settings.MpTextColor);
        }

        private void DrawGlobePercent(bool toggle, double percent, Vector2 pos, Color backColor, Color textColor)
        {
            if (!toggle)
                return;

            var text = ((int)Math.Round(percent * 100.0)).ToString(CultureInfo.InvariantCulture);
            using (Graphics.SetTextScale(Settings.GlobeFontScale))
            {
                var drawText = new Vector2();
                if (!String.IsNullOrEmpty(Settings.GlobeFont))
                {
                    drawText = Graphics.DrawText(text, pos, textColor, Settings.GlobeFont, FontAlign.Center | FontAlign.VerticalCenter);
                }
                else
                {
                    drawText = Graphics.DrawText(text, pos, textColor, FontAlign.Center | FontAlign.VerticalCenter);
                }

                var rectangle = new RectangleF(pos.X - drawText.X/2, pos.Y - drawText.Y/2, drawText.X, drawText.Y);

                Graphics.DrawBox(rectangle, backColor);
            }
            
        }

        public class LocalPlayer
        {
            public static Entity Entity => Entity.Player;

            private static Player GetPlayerComponent() =>
                Entity != null && Entity.TryGetComponent(out Player playerComp) ? playerComp : null;

            public static long Experience => GetPlayerComponent()?.XP ?? 0;

            public static string Name => GetPlayerComponent()?.PlayerName ?? string.Empty;

            public static int Level => GetPlayerComponent()?.Level ?? 0;

            public static Life Health => Entity?.GetComponent<Life>();

            public static int Kills => MainPlugin?.Kills ?? 0;

            public static AreaInstance Area => MainPlugin?.GameController.Area.CurrentArea;

            public static uint AreaHash => MainPlugin.GameController.Game.IngameState.Data.CurrentAreaHash;
        }
    }
}