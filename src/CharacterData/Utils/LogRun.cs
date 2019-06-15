// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.LogRun
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CharacterData.Core;
using Newtonsoft.Json;
using PoeHUD.Controllers;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;

namespace CharacterData.Utils
{
    public class LogRun
    {
        public LogRun()
        {
            PluginDirectory = Main.BasePluginDirectory;
        }

        public string PluginDirectory { get; set; }

        public LevelLogs LevelLogss { get; set; }

        public void Message(int timer = 600)
        {
            BasePlugin.API.LogMessage(DataToString(false), timer);
        }

        public string DataToString(bool full = false)
        {
            try
            {
                var str1 = "";
                if (full)
                    str1 += string.Format("{0}: ", DateTime.Now.ToString());
                var str2 = str1 + string.Format(
                               "[Level: {0} ({1:N2}%)] [Gained XP: {2:#,##0} ({3}%)] [Area Dif: {5}] [Areas ETA: {4}/{6}]",
                                LocalPlayer.Level, Main.Instance.Progress(),
                                Main.Instance.ExperienceGained(), Main.Instance.LevelPercentGained(),
                                Main.Instance.RunsToNextLevel(),
                               LocalPlayer.Level - Main.API.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                                   ? string.Format("+{0}",
                                       LocalPlayer.Level - Main.API.GameController.Game.IngameState.Data.CurrentAreaLevel)
                                   : (LocalPlayer.Level - Main.API.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                                Main.Instance.TotalRunsToNextLevel());
                if (LocalPlayer.Name == Main.Instance.JoinName && Main.Instance.JoinLevel < LocalPlayer.Level)
                    str2 += string.Format(" [Level Ups: {0}]", LocalPlayer.Level - Main.Instance.JoinLevel);
                if (full)
                    str2 += string.Format(" [Area: {0}] [Area Time: {1}]", Main.Instance.JoinArea.DisplayName,
                        Main.Instance.RunTime());
                return str2;
            }
            catch
            {
                return "";
            }
        }

        public bool NewEntry()
        {
            try
            {
                var entry = new Entry
                {
                    Id = DataToString(true),
                    Info = new Info
                    {
                        Date = DateTime.Now,
                        Level = new Level
                        {
                            CurrentLevel = LocalPlayer.Level,
                            CurrentLevelPercent = Main.Instance.Progress(),
                            ExperiencedGained = Main.Instance.ExperienceGained(),
                            ExperienceGainedPercent = Main.Instance.LevelPercentGained(),
                            LevelUps = (int)(LocalPlayer.Level - Main.Instance.JoinLevel)
                        },
                        Area = new Area
                        {
                            Name = Main.Instance.JoinArea.DisplayName,
                            LevelDifference = LocalPlayer.Level - Main.Instance.JoinArea.Area.MonsterLevel > 0
                                ? string.Format("+{0}", LocalPlayer.Level - Main.Instance.JoinArea.Area.MonsterLevel)
                                : (LocalPlayer.Level - Main.Instance.JoinArea.Area.MonsterLevel).ToString(),
                            TimeSpent = Main.Instance.RunTime(),
                            SameAreaEta = new SameAreaEta
                            {
                                Left = Main.Instance.RunsToNextLevel(),
                                TotalForLevel = Main.Instance.TotalRunsToNextLevel()
                            }
                        }
                    }
                };

                var playerName = LocalPlayer.Name;

                // shit fix for null player name
                if (string.IsNullOrEmpty(playerName))
                {
                    while (string.IsNullOrEmpty(GameController.Instance.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName))
                    {
                        Thread.Sleep(5);
                        BasePlugin.LogError("CharacterData: Null or Empty Name", 10);
                    }
                    playerName = GameController.Instance.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName;
                }

                var path1 = string.Format("{0}\\Level Entry\\{1}\\", PluginDirectory, playerName);

                var path2 = string.Format("{0}JsonLog.json", path1);
                var directoryName = Path.GetDirectoryName(path1);
                var levelLogs = new LevelLogs
                {
                    Entries = new List<Entry>()
                };
                if (!Directory.Exists(directoryName) && directoryName != null)
                    Directory.CreateDirectory(directoryName);
                try
                {
                    var str = File.ReadAllText(path2);
                    if (string.IsNullOrEmpty(str.Trim()))
                        throw new Exception();
                    if (str == "null")
                        throw new Exception();
                    levelLogs = JsonConvert.DeserializeObject<LevelLogs>(str);
                }
                catch
                {
                }

                levelLogs.Entries.Add(entry);
                var str1 = JsonConvert.SerializeObject(levelLogs, (Formatting) 1);
                using (var streamWriter = new StreamWriter(File.Create(path2)))
                {
                    streamWriter.Write(str1);
                }
            }
            catch (Exception ex)
            {
                //BasePlugin.LogError("Plugin error save json!\n" + ex, 3f);
            }

            return true;
        }

        public string NeatString(bool full = false)
        {
            try
            {
                var str = "";
                if (full)
                    str += string.Format("{0:HH:mm:ss}: ", DateTime.Now);
                return str + string.Format(
                           "Level: {0} ({1:N2}%){7}Area Difference: {5}{7}XP gained: {2:#,##0} ({3}%){7}Area Until Level: {4}/{6}",
                            LocalPlayer.Level, Main.Instance.Progress(),
                            Main.Instance.ExperienceGained(), Main.Instance.LevelPercentGained(),
                            Main.Instance.RunsToNextLevel(),
                           LocalPlayer.Level - Main.API.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                               ? string.Format("+{0}", LocalPlayer.Level - BasePlugin.API.GameController.Game.IngameState.Data.CurrentAreaLevel)
                               : (LocalPlayer.Level - Main.API.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                            Main.Instance.TotalRunsToNextLevel(), Environment.NewLine) +
                       string.Format("{1}Level Ups: {0}", LocalPlayer.Level - Main.Instance.JoinLevel,
                           Environment.NewLine);
            }
            catch
            {
                return "";
            }
        }

        private static bool MakeFolder(DirectoryInfo directory)
        {
            try
            {
                if (directory.Parent != null && !directory.Parent.Exists)
                    MakeFolder(directory.Parent);
                directory.Create();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Save()
        {
            var playerName = LocalPlayer.Name;

            // shit fix for null player name
            if (string.IsNullOrEmpty(playerName))
            {
                while (string.IsNullOrEmpty(GameController.Instance.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName))
                {
                    Thread.Sleep(5);
                    BasePlugin.LogError("CharacterData: Null or Empty Name", 10);
                }
                playerName = GameController.Instance.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName;
            }

            var path = string.Format("{0}\\Level Logs\\{1}", PluginDirectory, playerName);
            if (!MakeFolder(new DirectoryInfo(path)))
            {
                BasePlugin.LogError("Failed to make Directoryies", 10f);
                return false;
            }

            try
            {
                File.AppendAllText(string.Format("{0}\\Log.txt", path), DataToString(true) + Environment.NewLine);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}