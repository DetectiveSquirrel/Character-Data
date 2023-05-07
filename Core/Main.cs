// Decompiled with JetBrains decompiler
// Type: CharacterData.Core.Main
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using CharacterData.Long_Stuff;
using CharacterData.Utils;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DeployedObject = ExileCore.PoEMemory.Components.DeployedObject;
using Vector2 = System.Numerics.Vector2;

namespace CharacterData.Core
{
    public class Core : BaseSettingsPlugin<Settings>
    {

        public static Core MainPlugin { get; set; }

        public List<Entity> PlayerEntities { get; set; } = new List<Entity>();

        public static List<PartyElementWindow> PlayerInPartyDraw { get; set; } = new List<PartyElementWindow>();

        public static string BaseConfigDirectory { get; set; }

        public static InstanceSnapshot Instance { get; set; }

        public static LogRun Run { get; set; }

        private object FireRes { get; set; }

        private object FireResTotal { get; set; }

        public int MaxFireRes { get; private set; }

        private object ColdRes { get; set; }

        private object ColdResTotal { get; set; }

        public int MaxColdRes { get; private set; }

        private object LightRes { get; set; }

        private object LightResTotal { get; set; }

        public int MaxLightRes { get; private set; }

        private object ChaosRes { get; set; }

        private object ChaosResTotal { get; set; }

        public int MaxChaosRes { get; private set; }

        public int Kills { get; private set; }

        public List<Buff> buffs;

        private static bool ShouldLog => LocalPlayer.Experience != Instance.JoinExperience || LocalPlayer.Kills != Instance.JoinKills;

        public int TryGetStat(string playerStat, Entity entity)
        {
            return !entity.GetComponent<Stats>().StatDictionary.TryGetValue((GameStat)GameController.Files.Stats.records[playerStat].ID, out var statValue) ? 0 : statValue;
        }

        public int TryGetStat(string playerStat)
        {
            return !GameController.EntityListWrapper.Player.Stats.TryGetValue((GameStat)GameController.Files.Stats.records[playerStat].ID, out var statValue) ? 0 : statValue;
        }

        public override void AreaChange(AreaInstance area)
        {
            //base.AreaChange(area);
            ResetData();
        }

        private void ResetData()
        {
            LogRun();

            //GameController.Area.ForceRefreshArea(true);
            //yield return new WaitFunction(() => !Entity.Player.IsValid);
            Instance = new InstanceSnapshot();
            Run = new LogRun();

        }

        public override bool Initialise()
        {
            BaseConfigDirectory = ConfigDirectory;
            MainPlugin = this;
            Kills = TryGetStat("character_kill_count");
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
                        nameToKillsList.Add($"{playerComp.PlayerName,-23} ({playerComp.Level})", TryGetStat("character_kill_count", entity));
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

            Kills = TryGetStat("character_kill_count");

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

            var playerEntities = GameController.EntityListWrapper.ValidEntitiesByType[EntityType.Player];
            PlayerInPartyDraw = PartyElements.GetPlayerInfoElementList(playerEntities, Settings.PartyElement.Value);

            foreach (var partyElementWindow in PlayerInPartyDraw)
            {
                var fontSize = 13;
                if (ShouldDrawPartyElement(partyElementWindow))
                {
                    var statCount = 1;

                    var playerComp = partyElementWindow.Data.PlayerEntity.GetComponent<Player>();
                    var levelString = $"Level: {playerComp.Level} ({Progress(partyElementWindow.Data.PlayerEntity):N2}%)";

                    var FireRes = TryGetStat("fire_damage_resistance_%", partyElementWindow.Data.PlayerEntity);
                    var fireString = $"Fire: {FireRes}";

                    var ColdRes = TryGetStat("cold_damage_resistance_%", partyElementWindow.Data.PlayerEntity);
                    var coldString = $"Cold: {ColdRes}";

                    var LightRes = TryGetStat("lightning_damage_resistance_%", partyElementWindow.Data.PlayerEntity);
                    var lightString = $"Light: {LightRes}";

                    var ChaosRes = TryGetStat("chaos_damage_resistance_%", partyElementWindow.Data.PlayerEntity);
                    var chaosString = $"Chaos: {ChaosRes}";

                    var lifeComp = partyElementWindow.Data.PlayerEntity.GetComponent<Life>();
                    var lifeString = $"HP: {lifeComp.CurHP} / ES {lifeComp.CurES}";

                    var actionComp = partyElementWindow.Data.PlayerEntity.GetComponent<Actor>();
                    var actorString = $"Action: {actionComp.Action}";

                    var killComp = partyElementWindow.Data.PlayerEntity.GetComponent<Actor>();
                    var killString = $"Kills: {string.Format("{0:#,##0}", TryGetStat("character_kill_count", partyElementWindow.Data.PlayerEntity))}";

                    var xy = partyElementWindow.Element.GetClientRect().TopRight;
                    var LevelText = Graphics.DrawText(
                        levelString,
                        xy,
                        Settings.LevelTextColor);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(fireString,
                        xy,
                        Settings.FireResistanceColor);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(coldString,
                        xy,
                        Settings.ColdResistanceColor);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(lightString,
                        xy,
                        Settings.LightningResistanceColor);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(chaosString,
                        xy,
                        Settings.ChaosResistanceColor);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(lifeString,
                        xy,
                        Color.White);

                    statCount++;
                    xy.Y += fontSize;
                    Graphics.DrawText(killString,
                        xy,
                        Color.White);

                    var partyinfoRec = new RectangleF(partyElementWindow.Element.GetClientRect().TopRight.X, partyElementWindow.Element.GetClientRect().TopRight.Y, LevelText.X, fontSize * statCount);
                    Graphics.DrawBox(partyinfoRec, new Color(0, 0, 0, 185));

                }
            }
        }

        private bool ShouldDrawPartyElement(PartyElementWindow partyElementWindow)
        {
            return partyElementWindow.Data?.PlayerEntity?.IsValid != null
                   && !GameController.Game.IngameState.IngameUi.OpenLeftPanel.IsVisibleLocal
                   && !GameController.IngameState.IngameUi.ChatTitlePanel.IsVisibleLocal
                   && GameController.IngameState.IngameUi.ChatPanel.IndexInParent != 140
                   && !GameController.IngameState.IngameUi.Atlas.IsVisibleLocal
                   && !GameController.IngameState.IngameUi.TreePanel.IsVisibleLocal;
        }

        public double Progress(Entity entity)
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

            FireRes = TryGetStat("fire_damage_resistance_%");
            FireResTotal = TryGetStat("uncapped_fire_damage_resistance_%");
            MaxFireRes = TryGetStat("maximum_fire_damage_resistance_%") != 0 ? TryGetStat("maximum_fire_damage_resistance_%") : 75;

            ColdRes = TryGetStat("cold_damage_resistance_%");
            ColdResTotal = TryGetStat("uncapped_cold_damage_resistance_%");
            MaxColdRes = TryGetStat("maximum_cold_damage_resistance_%") != 0 ? TryGetStat("maximum_cold_damage_resistance_%") : 75;

            LightRes = TryGetStat("lightning_damage_resistance_%");
            LightResTotal = TryGetStat("uncapped_lightning_damage_resistance_%");
            MaxLightRes = TryGetStat("maximum_lightning_damage_resistance_%") != 0 ? TryGetStat("maximum_lightning_damage_resistance_%") : 75;

            ChaosRes = TryGetStat("chaos_damage_resistance_%");
            ChaosResTotal = TryGetStat("uncapped_chaos_damage_resistance_%");
            MaxChaosRes = TryGetStat("maximum_chaos_damage_resistance_%") != 0 ? TryGetStat("maximum_chaos_damage_resistance_%") : 75;

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
            Graphics.DrawText($"{"Kills:",-8}{string.Format("{0:#,##0}", Core.LocalPlayer.Kills)}", new Vector2(Settings.ResistanceX, Settings.ResistanceY + (Settings.ResistanceTextSize * 4)), Settings.KillsColor, FontAlign.Left);
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
            {
                return;
            }

            var newList = GameController.Player.GetComponent<Actor>().DeployedObjects;

            try
            {
                var actorDeployedObjects = DeployedObjectsSorted(newList).Reverse();
                var loopCount = 0;

                foreach (var actorDeployedObject in actorDeployedObjects)
                {
                    Graphics.DrawText($@"{actorDeployedObject.Value,-2}: {actorDeployedObject.Key}",
                        new Vector2(Settings.ActorObjectX, Settings.ActorObjectY - (Settings.ResistanceTextSize * loopCount)), Settings.ActorObjectColor,
                        FontAlign.Left);
                    loopCount++;
                }
            }
            catch (Exception e)
            {
                LogError(e.ToString(), 10);
            }
        }

        public SortedDictionary<string, int> DeployedObjectsSorted(List<DeployedObject> objectList)
        {
            var listCopy = objectList;

            var actorDeployedObjects = new SortedDictionary<string, int>();

            foreach (var deployedObject in listCopy.Where(deployedObject => deployedObject.Entity != null && deployedObject.Entity.Address != 0 && !string.IsNullOrEmpty(deployedObject.Entity.RenderName)))
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

        private static int ResistanceDifference(object cappedResistance, object uncappedResistance, object maximumCappedResistance)
        {
            return (int)uncappedResistance <= (int)cappedResistance ? (int)cappedResistance - (int)maximumCappedResistance : (int)uncappedResistance - (int)cappedResistance;
        }

        private void GlobePercents()
        {
            if ((bool)Settings.HealthToggle)
            {
                var text = double.Parse((Math.Round(LocalPlayer.Health.HPPercentage, 3) * 100.0).ToString(CultureInfo.InvariantCulture).Split('.')[0]).ToString(CultureInfo.InvariantCulture);
                var rectangle = new RectangleF(Settings.HpPositionX, Settings.HpPositionY, 50f, 27f);
                Graphics.DrawBox(rectangle, Settings.HpBackColor);
                Graphics.DrawText(text, new Vector2(rectangle.X + (rectangle.Width / 2f), rectangle.Y), Settings.HpTextColor, FontAlign.Center);
            }

            if (!(bool)Settings.ManaToggle)
            {
                return;
            }

            var text1 = double.Parse((Math.Round(LocalPlayer.Health.MPPercentage, 3) * 100.0).ToString(CultureInfo.InvariantCulture).Split('.')[0]).ToString(CultureInfo.InvariantCulture);
            var rectangle1 = new RectangleF(Settings.MpPositionX, Settings.MpPositionY, 50f, 27f);
            Graphics.DrawBox(rectangle1, Settings.MpBackColor);
            Graphics.DrawText(text1, new Vector2(rectangle1.X + (rectangle1.Width / 2f), rectangle1.Y), Settings.MpTextColor, FontAlign.Center);
        }

        public class LocalPlayer
        {
            public static Entity Entity => Entity.Player;

            public static long Experience => Entity.GetComponent<Player>().XP;

            public static string Name => Entity.GetComponent<Player>().PlayerName;

            public static int Level => Entity.GetComponent<Player>().Level;

            public static Stats Stat => Entity.GetComponent<Stats>();

            public static Life Health => Entity.GetComponent<Life>();

            public static int Kills => MainPlugin.Kills;

            public static AreaInstance Area => MainPlugin.GameController.Area.CurrentArea;

            public static uint AreaHash => MainPlugin.GameController.Game.IngameState.Data.CurrentAreaHash;

        }
    }
}
