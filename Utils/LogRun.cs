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
using ExileCore;
using ExileCore.PoEMemory.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CharacterData.Utils
{
    public class LogRun
    {
        public LogRun()
        {
            LogDirectory = Core.Core.BaseConfigDirectory;
        }

        public string LogDirectory { get; set; }

        public LevelLogs LevelLogss { get; set; }

        public void Message(int timer = 600)
        {
            Core.Core.MainPlugin.LogMessage(DataToString(false), timer);
        }

        public string DataToString(bool full = false)
        {
            try
            {
                var str1 = "";
                if (full)
                    str1 += string.Format("{0}: ", DateTime.Now.ToString());
                var str2 = str1 + string.Format(
                               "[Level: {0} ({1:N2}%)] [Gained XP: {2:#,##0} ({3}%)] [Area Dif: {5}] [Areas ETA: {4}/{6}] [Kills: {7:#,##0} | Total Kills: {8:#,##0}]",
                                Core.Core.LocalPlayer.Level, Core.Core.Instance.Progress(),
                                Core.Core.Instance.ExperienceGained(), Core.Core.Instance.LevelPercentGained(),
                               Core.Core.Instance.RunsToNextLevel(),
                               Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                                   ? string.Format("+{0}",
                                       Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel)
                                   : (Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                               Core.Core.Instance.TotalRunsToNextLevel(),
                           Core.Core.LocalPlayer.Kills - Core.Core.Instance.JoinKills, Core.Core.LocalPlayer.Kills);
                if (Core.Core.LocalPlayer.Name == Core.Core.Instance.JoinName && Core.Core.Instance.JoinLevel < Core.Core.LocalPlayer.Level)
                    str2 += string.Format(" [Level Ups: {0}]", Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinLevel);
                if (full)
                    str2 += string.Format(" [Area: {0}] [Area Time: {1}]", Core.Core.Instance.JoinArea.DisplayName,
                        Core.Core.Instance.RunTime());
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
                            CurrentLevel = Core.Core.LocalPlayer.Level,
                            CurrentLevelPercent = Core.Core.Instance.Progress(),
                            ExperiencedGained = Core.Core.Instance.ExperienceGained(),
                            ExperienceGainedPercent = Core.Core.Instance.LevelPercentGained(),
                            LevelUps = (int)(Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinLevel),
                            AreaKills = (Core.Core.LocalPlayer.Kills - Core.Core.Instance.JoinKills),
                            TotalKills = Core.Core.LocalPlayer.Kills
                        },
                        Area = new Area
                        {
                            Name = Core.Core.Instance.JoinArea.DisplayName,
                            LevelDifference = Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinArea.Area.MonsterLevel > 0
                                ? string.Format("+{0}", Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinArea.Area.MonsterLevel)
                                : (Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinArea.Area.MonsterLevel).ToString(),
                            TimeSpent = Core.Core.Instance.RunTime(),
                            SameAreaEta = new SameAreaEta
                            {
                                Left = Core.Core.Instance.RunsToNextLevel(),
                                TotalForLevel = Core.Core.Instance.TotalRunsToNextLevel()
                            }
                        }
                    }
                };

                var playerName = Core.Core.LocalPlayer.Name;

                // shit fix for null player name
                if (string.IsNullOrEmpty(playerName))
                {
                    while (string.IsNullOrEmpty(Core.Core.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName))
                    {
                        Thread.Sleep(5);
                        Core.Core.MainPlugin.LogError("CharacterData: Null or Empty Name", 10);
                    }
                    playerName = Core.Core.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName;
                }

                var path1 = string.Format("{0}\\Level Entry\\{1}\\", LogDirectory, playerName);

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
                           "Level: {0} ({1:N2}%){7}Area Difference: {5}{7}XP gained: {2:#,##0} ({3}%){7}Area Until Level: {4}/{6}{7}Level Ups: {8}{7}Area Kills: {9:#,##0}",
                            Core.Core.LocalPlayer.Level, Core.Core.Instance.Progress(),
                           Core.Core.Instance.ExperienceGained(), Core.Core.Instance.LevelPercentGained(),
                           Core.Core.Instance.RunsToNextLevel(),
                           Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                               ? string.Format("+{0}", Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel)
                               : (Core.Core.LocalPlayer.Level - Core.Core.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                           Core.Core.Instance.TotalRunsToNextLevel(), Environment.NewLine, Core.Core.LocalPlayer.Level - Core.Core.Instance.JoinLevel,
                           Core.Core.LocalPlayer.Kills - Core.Core.Instance.JoinKills);
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
            var playerName = Core.Core.LocalPlayer.Name;

            // shit fix for null player name
            if (string.IsNullOrEmpty(playerName))
            {
                while (string.IsNullOrEmpty(Core.Core.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName))
                {
                    Thread.Sleep(5);
                    Core.Core.MainPlugin.LogError("CharacterData: Null or Empty Name", 10);
                }
                playerName = Core.Core.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName;
            }

            var path = string.Format("{0}\\Level Logs\\{1}", LogDirectory, playerName);
            if (!MakeFolder(new DirectoryInfo(path)))
            {
                Core.Core.MainPlugin.LogError("Failed to make Directoryies", 10f);
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