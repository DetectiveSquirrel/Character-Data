// Decompiled with JetBrains decompiler
// Type: CharacterData.Core.Main
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using CharacterData.Long_Stuff;
using CharacterData.Utils;
using ImGuiNET;
using Newtonsoft.Json;
using PoeHUD.Controllers;
using PoeHUD.Framework;
using PoeHUD.Framework.Helpers;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using SharpDX;
using SharpDX.Direct3D9;

namespace CharacterData.Core
{
    public class Main : BaseSettingsPlugin<Settings>
    {
        public Main()
        {
            PluginName = "Character Data";
        }

        public static PartyElements partyStuff { get; set; }

        public List<EntityWrapper> PlayerEntities { get; set; } = new List<EntityWrapper>();

        public static List<PartyElementWindow> PlayerInPartyDraw { get; set; } = new List<PartyElementWindow>();

        public static string BasePluginDirectory { get; set; }

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

        private static bool ShouldLog
        {
            get
            {
                return LocalPlayer.Experience != Instance.JoinExperience;
            }
        }

        public int TryGetStat(string playerStat, EntityWrapper entity)
        {
            return !entity.GetComponent<Stats>().StatDictionary.TryGetValue(GameController.Files.Stats.records[playerStat].ID, out var statValue) ? 0 : statValue;
        }

        public int TryGetStat(string playerStat)
        {
            return !GameController.EntityListWrapper.PlayerStats.TryGetValue(GameController.Files.Stats.records[playerStat].ID, out var statValue) ? 0 : statValue;
        }

        private void AreaChange()
        {
            new Coroutine(ResetData(), "CharacterData", "Character Data").Run();
        }

        private IEnumerator ResetData()
        {
            LogRun();
            yield return new WaitFunction(() => !GameController.Game.IngameState.Data.LocalPlayer.IsValid);
            Instance = new InstanceSnapshot();
            Run = new LogRun();
        }

        public override void Initialise()
        {
            BasePluginDirectory = PluginDirectory;
            GameController.Area.OnAreaChange += area => AreaChange();
            Instance = new InstanceSnapshot();
            Run = new LogRun();
            partyStuff = new PartyElements(this);
        }

        public override void EntityAdded(EntityWrapper entityWrapper)
        {
            if (entityWrapper.HasComponent<Player>())
            {
                PlayerEntities.Add(entityWrapper);
            }
        }

        public override void EntityRemoved(EntityWrapper entityWrapper)
        {
            PlayerEntities.Remove(entityWrapper);
        }

        public override void DrawSettingsMenu()
        {
            bool outBool;
            if (ImGuiExtension.Checkbox(Settings.Resolution ? "Background: Plain Square" : "Background: Image suitable for 1920x1080 only", Settings.Resolution, out outBool) && ImGui.TreeNode("Background Settings"))
            {
                Settings.ResolutionTop.Value = ImGuiExtension.IntSlider("Top", Settings.ResolutionTop);
                Settings.ResolutionBottom.Value = ImGuiExtension.IntSlider("Bottom", Settings.ResolutionBottom);
                Settings.ResolutionLeft.Value = ImGuiExtension.IntSlider("Left", Settings.ResolutionLeft);
                Settings.ResolutionRight.Value = ImGuiExtension.IntSlider("Right", Settings.ResolutionRight);
                Settings.LevelBackColor = ImGuiExtension.ColorPicker("Background Color", Settings.LevelBackColor);
                ImGui.Separator();
                ImGui.TreePop();
            }

            Settings.Resolution.Value = outBool;
            if (ImGui.TreeNode("Health/Mana Boxes"))
            {
                ImGui.SetNextTreeNodeOpen(true, Condition.Once);
                if (ImGui.TreeNode("Health %"))
                {
                    Settings.HealthToggle = ImGuiExtension.Checkbox("Health % (" + (Settings.HealthToggle ? "Show" : "Hide") + ")", Settings.HealthToggle);
                    Settings.HpPositionX.Value = ImGuiExtension.IntSlider("HP X", Settings.HpPositionX);
                    Settings.HpPositionY.Value = ImGuiExtension.IntSlider("HP Y", Settings.HpPositionY);
                    Settings.HpTextColor = ImGuiExtension.ColorPicker("HP Text", Settings.HpTextColor);
                    Settings.HpBackColor = ImGuiExtension.ColorPicker("HP Background", Settings.HpBackColor);
                    ImGui.TreePop();
                }

                ImGui.SetNextTreeNodeOpen(true, Condition.Once);
                if (ImGui.TreeNode("Mana %"))
                {
                    Settings.ManaToggle = ImGuiExtension.Checkbox("Mana % (" + (Settings.ManaToggle ? "Show" : "Hide") + ")", Settings.ManaToggle);
                    Settings.MpPositionX.Value = ImGuiExtension.IntSlider("MP X", Settings.MpPositionX);
                    Settings.MpPositionY.Value = ImGuiExtension.IntSlider("MP Y", Settings.MpPositionY);
                    Settings.MpTextColor = ImGuiExtension.ColorPicker("MP Text", Settings.MpTextColor);
                    Settings.MpBackColor = ImGuiExtension.ColorPicker("MP Background", Settings.MpBackColor);
                    ImGui.Separator();
                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Player Resistance UI"))
            {
                Settings.Resistances = ImGuiExtension.Checkbox("Player Resistance UI (" + (Settings.Resistances ? "Show" : "Hide") + ")", Settings.Resistances);
                Settings.ResistanceX.Value = ImGuiExtension.IntSlider("Res X", Settings.ResistanceX);
                Settings.ResistanceY.Value = ImGuiExtension.IntSlider("Res Y", Settings.ResistanceY);
                Settings.ResistanceTextSize = ImGuiExtension.IntSlider("Res Text Size", Settings.ResistanceTextSize, 1, 40);
                ImGui.Spacing();
                Settings.FireResistanceColor = ImGuiExtension.ColorPicker("Fire Res Color", Settings.FireResistanceColor);
                Settings.ColdResistanceColor = ImGuiExtension.ColorPicker("Cold Res Color", Settings.ColdResistanceColor);
                Settings.LightningResistanceColor = ImGuiExtension.ColorPicker("Light Res Color", Settings.LightningResistanceColor);
                Settings.ChaosResistanceColor = ImGuiExtension.ColorPicker("Chaos Res Color", Settings.ChaosResistanceColor);
                ImGui.Separator();
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Delve Info UI"))
            {
                Settings.Delveinfo = ImGuiExtension.Checkbox("Delve Info UI (" + (Settings.Delveinfo ? "Show" : "Hide") + ")", Settings.Delveinfo);
                Settings.DelveinfoX.Value = ImGuiExtension.IntSlider("Delveinfo X", Settings.DelveinfoX);
                Settings.DelveinfoY.Value = ImGuiExtension.IntSlider("Delveinfo Y", Settings.DelveinfoY);
                Settings.DelveinfoTextSize = ImGuiExtension.IntSlider("Delveinfo Text Size", Settings.DelveinfoTextSize, 1, 40);
                ImGui.Spacing();
                Settings.DelveinfoSulphiteColor = ImGuiExtension.ColorPicker("Sulphite Color", Settings.DelveinfoSulphiteColor);
                Settings.DelveinfoAzuriteColor = ImGuiExtension.ColorPicker("Azurite Color", Settings.DelveinfoAzuriteColor);
                ImGui.Separator();
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Instance Information"))
            {
                Settings.LevelToggle = ImGuiExtension.Checkbox("Instance Information (" + (Settings.LevelToggle ? "Show" : "Hide") + ")", Settings.LevelToggle);
                Settings.SaveToFile.Value = ImGuiExtension.Checkbox("Log To File", Settings.SaveToFile);
                Settings.LastAreaDuration.Value = ImGuiExtension.IntSlider("Last Area Log In Seconds", Settings.LastAreaDuration);
                ImGui.Spacing();
                Settings.LevelPositionX.Value = ImGuiExtension.IntSlider("Instance Info X", Settings.LevelPositionX);
                Settings.LevelPositionY.Value = ImGuiExtension.IntSlider("Instance Info Y", Settings.LevelPositionY);
                Settings.LevelTextColor = ImGuiExtension.ColorPicker("Instance Info Text Color", Settings.LevelTextColor);
                Settings.LevelTextSize.Value = ImGuiExtension.IntSlider("Instance Info Text Size", Settings.LevelTextSize);
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Experience Bar"))
            {
                Settings.ExperienceBar = ImGuiExtension.Checkbox("Experience Bar Percent (" + (Settings.ExperienceBar ? "Show" : "Hide") + ")", Settings.ExperienceBar);
                ImGui.TreePop();
            }

            ImGui.BeginChild("CharacteLogs", true, WindowFlags.AlwaysVerticalScrollbar);
            var levelLogs = new LevelLogs();
            var path1 = $"{PluginDirectory}\\Level Entry\\{LocalPlayer.Name}\\";
            var path2 = $"{path1}JsonLog.json";
            var directoryName = Path.GetDirectoryName(path1);
            if (!Directory.Exists(directoryName) && directoryName != null)
                Directory.CreateDirectory(directoryName);
            try
            {
                var str = File.ReadAllText(path2);
                if (string.IsNullOrEmpty(str.Trim()))
                    throw new Exception();
                if (str == "null")
                    throw new Exception();
                var m0 = JsonConvert.DeserializeObject<LevelLogs>(str);
                m0.Entries.Reverse();
                foreach (var entry in m0.Entries)
                    if (ImGui.TreeNode(entry.Id))
                    {
                        ImGui.SetNextTreeNodeOpen(true, Condition.Appearing);
                        if (ImGui.TreeNode("Info"))
                        {
                            ImGui.Text($"Date: {entry.Info.Date}");
                            ImGui.SetNextTreeNodeOpen(true, Condition.Appearing);
                            if (ImGui.TreeNode("Level"))
                            {
                                ImGuiNative.igIndent();
                                ImGui.Text($"Current LvL: {entry.Info.Level.CurrentLevel}");
                                ImGui.Text($"Current LvL %%: {entry.Info.Level.CurrentLevelPercent}");
                                ImGui.Text($"Exp Gained: {entry.Info.Level.ExperiencedGained}");
                                ImGui.Text($"Exp Gained %%: {entry.Info.Level.ExperienceGainedPercent}");
                                ImGui.Text($"Level Ups: {entry.Info.Level.LevelUps}");
                                ImGuiNative.igUnindent();
                                ImGui.TreePop();
                            }

                            ImGui.SetNextTreeNodeOpen(true, Condition.Appearing);
                            if (ImGui.TreeNode("Area"))
                            {
                                ImGuiNative.igIndent();
                                ImGui.Text($"Area: {entry.Info.Area.Name}");
                                ImGui.Text($"LvL Difference: {entry.Info.Area.LevelDifference}");
                                ImGui.Text($"Run Time: {entry.Info.Area.TimeSpent}");
                                ImGui.Text($"Same Area LvL ETA: {entry.Info.Area.SameAreaEta.Left}/{entry.Info.Area.SameAreaEta.TotalForLevel}");
                                ImGuiNative.igUnindent();
                                ImGui.TreePop();
                            }

                            ImGui.TreePop();
                        }

                        ImGui.TreePop();
                    }
            }
            catch
            {
            }

            ImGui.EndChild();
        }

        public void ProcessLevelLogs()
        {
        }

        public override void Render()
        {
            Background();
            GlobePercents();
            DrawExperienceData();
            DrawResistances();
            DrawDelveInfo();
            DrawExperiencepercentbar();

            PlayerInPartyDraw = PartyElements.GetPlayerInfoElementList(PlayerEntities);

            foreach (var partyElementWindow in PlayerInPartyDraw)
            {
                var fontSize = 13;
                if (partyElementWindow.Data?.PlayerEntity?.IsValid != null && !GameController.Game.IngameState.IngameUi.OpenLeftPanel.IsVisible)
                {
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


                    var xy = partyElementWindow.Element.GetClientRect().TopRight;
                    var LevelText = Graphics.DrawText(
                        levelString,
                        fontSize,
                        xy,
                        Settings.LevelTextColor);

                    xy.Y += (fontSize);
                    Graphics.DrawText(fireString,
                        fontSize,
                        xy,
                        Settings.FireResistanceColor);

                    xy.Y += (fontSize);
                    Graphics.DrawText(coldString,
                        fontSize,
                        xy,
                        Settings.ColdResistanceColor);

                    xy.Y += (fontSize);
                    Graphics.DrawText(lightString,
                        fontSize,
                        xy,
                        Settings.LightningResistanceColor);

                    xy.Y += (fontSize);
                    var chaosText = Graphics.DrawText(chaosString,
                        fontSize,
                        xy,
                        Settings.ChaosResistanceColor);

                    var partyinfoRec = new RectangleF(partyElementWindow.Element.GetClientRect().TopRight.X, partyElementWindow.Element.GetClientRect().TopRight.Y, LevelText.Width, fontSize*5);
                    Graphics.DrawBox(partyinfoRec, new Color(0, 0, 0, 185));
                }
            }
        }
        public double Progress(EntityWrapper entity)
        {
            if (entity.InternalEntity.GetComponent<Player>().Level != 100)
                return (entity.InternalEntity.GetComponent<Player>().XP - (double)PlayerExperience.TotalExperience[entity.InternalEntity.GetComponent<Player>().Level]) /
                       PlayerExperience.NextExperience[entity.InternalEntity.GetComponent<Player>().Level] * 100.0;
            return 0.0;
        }

        private void Background()
        {
            if (!(bool) Settings.Resolution)
                Graphics.DrawPluginImage($"{PluginDirectory}\\Poe Status Bar.png", new RectangleF(550f, 957f, 821f, 104f));
            else
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
                return;
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
            if (!(bool) Settings.LevelToggle)
                return;
            Graphics.DrawText(Run.NeatString(), Settings.LevelTextSize.Value, new Vector2(Settings.LevelPositionX, Settings.LevelPositionY), Settings.LevelTextColor.Value);
        }

        private void DrawExperiencepercentbar()
        {
            if (!(bool) Settings.ExperienceBar)
                return;
            Graphics.DrawPluginImage($"{PluginDirectory}\\Poe Level Percents.png", new RectangleF(0.0f, 0.0f, 1920f, 1080f));
        }

        private void DrawResistances()
        {
            if (!(bool) Settings.Resistances)
                return;
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
            Graphics.DrawText($"Fire: {FireRes} ({(num1 > 0 ? "+" + num1 : num1.ToString())})", Settings.ResistanceTextSize, new Vector2(Settings.ResistanceX, Settings.ResistanceY), Settings.FireResistanceColor, FontDrawFlags.Right);
            Graphics.DrawText($"Cold: {ColdRes} ({(num2 > 0 ? "+" + num2 : num2.ToString())})", Settings.ResistanceTextSize, new Vector2(Settings.ResistanceX, Settings.ResistanceY + Settings.ResistanceTextSize), Settings.ColdResistanceColor, FontDrawFlags.Right);
            Graphics.DrawText($"Light: {LightRes} ({(num3 > 0 ? "+" + num3 : num3.ToString())})", Settings.ResistanceTextSize, new Vector2(Settings.ResistanceX, Settings.ResistanceY + Settings.ResistanceTextSize * 2), Settings.LightningResistanceColor, FontDrawFlags.Right);
            Graphics.DrawText($"Chaos: {ChaosRes} ({(num4 > 0 ? "+" + num4 : num4.ToString())})", Settings.ResistanceTextSize, new Vector2(Settings.ResistanceX, Settings.ResistanceY + Settings.ResistanceTextSize * 3), Settings.ChaosResistanceColor, FontDrawFlags.Right);
        }

        private void DrawDelveInfo()
        {
            if (!(bool) Settings.Delveinfo)
                return;

            var sulphiteCount = GameController.Game.IngameState.ServerData.CurrentSulphiteAmount;
            var azuriteCount = GameController.Game.IngameState.ServerData.CurrentAzuriteAmount;
            try
            {
                Graphics.DrawText($"{"Sulphite: ".PadRight(11, ' ')}{sulphiteCount:#,##0}", Settings.DelveinfoTextSize, new Vector2(Settings.DelveinfoX, Settings.DelveinfoY), Settings.DelveinfoSulphiteColor);
                Graphics.DrawText($"{"Azurite: ".PadRight(11, ' ')}{azuriteCount:#,##0}", Settings.DelveinfoTextSize, new Vector2(Settings.DelveinfoX, Settings.DelveinfoY + Settings.DelveinfoTextSize), Settings.DelveinfoAzuriteColor);
            }
            catch
            {
            }
        }

        private static int ResistanceDifference(object cappedResistance, object uncappedResistance, object maximumCappedResistance)
        {
            return (int) uncappedResistance <= (int) cappedResistance ? (int) cappedResistance - (int) maximumCappedResistance : (int) uncappedResistance - (int) cappedResistance;
        }

        private void GlobePercents()
        {
            if ((bool) Settings.HealthToggle)
            {
                var text = double.Parse((Math.Round(LocalPlayer.Health.HPPercentage, 3) * 100.0).ToString(CultureInfo.InvariantCulture).Split('.')[0]).ToString(CultureInfo.InvariantCulture);
                var rectangle = new RectangleF(Settings.HpPositionX, Settings.HpPositionY, 50f, 27f);
                Graphics.DrawBox(rectangle, Settings.HpBackColor);
                Graphics.DrawText(text, 25, new Vector2(rectangle.X + rectangle.Width / 2f, rectangle.Y), Settings.HpTextColor, FontDrawFlags.Center);
            }

            if (!(bool) Settings.ManaToggle)
                return;
            var text1 = double.Parse((Math.Round(LocalPlayer.Health.MPPercentage, 3) * 100.0).ToString(CultureInfo.InvariantCulture).Split('.')[0]).ToString(CultureInfo.InvariantCulture);
            var rectangle1 = new RectangleF(Settings.MpPositionX, Settings.MpPositionY, 50f, 27f);
            Graphics.DrawBox(rectangle1, Settings.MpBackColor);
            Graphics.DrawText(text1, 25, new Vector2(rectangle1.X + rectangle1.Width / 2f, rectangle1.Y), Settings.MpTextColor, FontDrawFlags.Center);
        }
    }
}