using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CharacterData.Utils
{
    public class LogRun
    {
        public LogRun()
        {
            LogDirectory = CharacterData.BaseConfigDirectory;
        }

        public string LogDirectory { get; set; }

        public LevelLogs LevelLogss { get; set; }

        public void Message(int timer = 600)
        {
            CharacterData.MainPlugin.LogMessage(DataToString(false), timer);
        }

        public string DataToString(bool full = false)
        {
            try
            {
                var str1 = "";
                if (full)
                {
                    str1 += string.Format("{0}: ", DateTime.Now.ToString());
                }

                var str2 = str1 + string.Format(
                               "[Level: {0} ({1:N2}%)] [Gained XP: {2:#,##0} ({3}%)] [Area Dif: {5}] [Areas ETA: {4}/{6}] [Kills: {7:#,##0} | Total Kills: {8:#,##0}]",
                                CharacterData.LocalPlayer.Level, InstanceSnapshot.Progress(),
                                CharacterData.Instance.ExperienceGained(), CharacterData.Instance.LevelPercentGained(),
                               CharacterData.Instance.RunsToNextLevel(),
                               CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                                   ? string.Format("+{0}",
                                       CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel)
                                   : (CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                               CharacterData.Instance.TotalRunsToNextLevel(),
                           CharacterData.LocalPlayer.Kills - CharacterData.Instance.JoinKills, CharacterData.LocalPlayer.Kills);
                if (CharacterData.LocalPlayer.Name == CharacterData.Instance.JoinName && CharacterData.Instance.JoinLevel < CharacterData.LocalPlayer.Level)
                {
                    str2 += string.Format(" [Level Ups: {0}]", CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinLevel);
                }

                if (full)
                {
                    str2 += string.Format(" [Area: {0}] [Area Time: {1}]", CharacterData.Instance.JoinArea.DisplayName,
                        CharacterData.Instance.RunTime());
                }

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
                            CurrentLevel = CharacterData.LocalPlayer.Level,
                            CurrentLevelPercent = InstanceSnapshot.Progress(),
                            ExperiencedGained = CharacterData.Instance.ExperienceGained(),
                            ExperienceGainedPercent = CharacterData.Instance.LevelPercentGained(),
                            LevelUps = (int)(CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinLevel),
                            AreaKills = CharacterData.LocalPlayer.Kills - CharacterData.Instance.JoinKills,
                            TotalKills = CharacterData.LocalPlayer.Kills
                        },
                        Area = new Area
                        {
                            Name = CharacterData.Instance.JoinArea.DisplayName,
                            LevelDifference = CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinArea.Area.MonsterLevel > 0
                                ? string.Format("+{0}", CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinArea.Area.MonsterLevel)
                                : (CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinArea.Area.MonsterLevel).ToString(),
                            TimeSpent = CharacterData.Instance.RunTime(),
                            SameAreaEta = new SameAreaEta
                            {
                                Left = CharacterData.Instance.RunsToNextLevel(),
                                TotalForLevel = CharacterData.Instance.TotalRunsToNextLevel()
                            }
                        }
                    }
                };

                var playerName = CharacterData.LocalPlayer.Name;

                // shit fix for null player name
                if (string.IsNullOrEmpty(playerName))
                {
                    while (string.IsNullOrEmpty(CharacterData.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<ExileCore.PoEMemory.Components.Player>().PlayerName))
                    {
                        Thread.Sleep(5);
                        CharacterData.MainPlugin.LogError("PlayerData: Null or Empty Name", 10);
                    }
                    playerName = CharacterData.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<ExileCore.PoEMemory.Components.Player>().PlayerName;
                }

                var path1 = string.Format("{0}\\Level Entry\\{1}\\", LogDirectory, playerName);

                var path2 = string.Format("{0}JsonLog.json", path1);
                var directoryName = Path.GetDirectoryName(path1);
                var levelLogs = new LevelLogs
                {
                    Entries = new List<Entry>()
                };
                if (!Directory.Exists(directoryName) && directoryName != null)
                {
                    Directory.CreateDirectory(directoryName);
                }

                try
                {
                    var str = File.ReadAllText(path2);
                    if (string.IsNullOrEmpty(str.Trim()))
                    {
                        throw new Exception();
                    }

                    if (str == "null")
                    {
                        throw new Exception();
                    }

                    levelLogs = JsonConvert.DeserializeObject<LevelLogs>(str);
                }
                catch
                {
                }

                levelLogs.Entries.Add(entry);
                var str1 = JsonConvert.SerializeObject(levelLogs, (Formatting)1);
                using (var streamWriter = new StreamWriter(File.Create(path2)))
                {
                    streamWriter.Write(str1);
                }
            }
            catch (Exception)
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
                {
                    str += string.Format("{0:HH:mm:ss}: ", DateTime.Now);
                }

                return str + string.Format(
                           "Level: {0} ({1:N2}%){7}Area Difference: {5}{7}XP gained: {2:#,##0} ({3}%){7}Area Until Level: {4}/{6}{7}Level Ups: {8}{7}Area Kills: {9:#,##0}",
                            CharacterData.LocalPlayer.Level, InstanceSnapshot.Progress(),
                           CharacterData.Instance.ExperienceGained(), CharacterData.Instance.LevelPercentGained(),
                           CharacterData.Instance.RunsToNextLevel(),
                           CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel > 0
                               ? string.Format("+{0}", CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel)
                               : (CharacterData.LocalPlayer.Level - CharacterData.MainPlugin.GameController.Game.IngameState.Data.CurrentAreaLevel).ToString(),
                           CharacterData.Instance.TotalRunsToNextLevel(), Environment.NewLine, CharacterData.LocalPlayer.Level - CharacterData.Instance.JoinLevel,
                           CharacterData.LocalPlayer.Kills - CharacterData.Instance.JoinKills);
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
                {
                    MakeFolder(directory.Parent);
                }

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
            var playerName = CharacterData.LocalPlayer.Name;

            // shit fix for null player name
            if (string.IsNullOrEmpty(playerName))
            {
                while (string.IsNullOrEmpty(CharacterData.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<ExileCore.PoEMemory.Components.Player>().PlayerName))
                {
                    Thread.Sleep(5);
                    CharacterData.MainPlugin.LogError("PlayerData: Null or Empty Name", 10);
                }
                playerName = CharacterData.MainPlugin.GameController.Game.IngameState.Data.LocalPlayer.GetComponent<ExileCore.PoEMemory.Components.Player>().PlayerName;
            }

            var path = string.Format("{0}\\Level Logs\\{1}", LogDirectory, playerName);
            if (!MakeFolder(new DirectoryInfo(path)))
            {
                CharacterData.MainPlugin.LogError("Failed to make Directoryies", 10f);
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