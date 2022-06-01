// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.InstanceSnapshot
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using CharacterData.Long_Stuff;
using ExileCore;

namespace CharacterData.Utils
{
    public class InstanceSnapshot
    {
        public InstanceSnapshot()
        {
            JoinExperience = Core.Core.LocalPlayer.Experience;
            JoinLevel = Core.Core.LocalPlayer.Level;
            JoinName = Core.Core.LocalPlayer.Name;
            JoinTime = DateTime.Now;
            JoinArea = Core.Core.LocalPlayer.Area;
            JoinAreaHash = Core.Core.LocalPlayer.AreaHash;
            JoinKills = Core.Core.LocalPlayer.Kills;
        }

        public long JoinExperience { get; set; }

        public long JoinLevel { get; set; }

        public string JoinName { get; set; }

        public DateTime JoinTime { get; set; }

        public AreaInstance JoinArea { get; set; }

        public uint JoinAreaHash { get; set; }

        public int JoinKills { get; set; }

        public double Progress()
        {
            if (Core.Core.LocalPlayer.Level != 100)
                return (Core.Core.LocalPlayer.Experience - (double) PlayerExperience.TotalExperience[Core.Core.LocalPlayer.Level]) / PlayerExperience.NextExperience[Core.Core.LocalPlayer.Level] * 100.0;
            return 0.0;
        }

        public long ExperienceGained()
        {
            return Core.Core.LocalPlayer.Experience - JoinExperience;
        }

        public long KillsInArea()
        {
            return Core.Core.LocalPlayer.Kills - JoinKills;
        }

        public string LevelPercentGained()
        {
            return (Core.Core.LocalPlayer.Level == 100
                ? 0.0
                : ExperienceGained() * 100.0 / PlayerExperience.NextExperience[Core.Core.LocalPlayer.Level]).ToString("N2");
        }

        public string RunsToNextLevel()
        {
            var d = Math.Round(
                (100.0 - Progress()) / 100.0 * PlayerExperience.NextExperience[Core.Core.LocalPlayer.Level] / ExperienceGained(),
                0, MidpointRounding.AwayFromZero);
            if (!double.IsInfinity(d))
                return d.ToString("N0");
            return "∞";
        }

        public string TotalRunsToNextLevel()
        {
            var d = Math.Round(PlayerExperience.NextExperience[Core.Core.LocalPlayer.Level] / (double) ExperienceGained(), 2,
                MidpointRounding.AwayFromZero);
            if (!double.IsInfinity(d))
                return d.ToString("N0");
            return "∞";
        }

        public string RunTime()
        {
            var totalSeconds = (int) (DateTime.Now - JoinTime).TotalSeconds;
            var num1 = totalSeconds % 60;
            var num2 = totalSeconds / 60;
            var num3 = num2 / 60;
            var num4 = num2 % 60;
            return string.Format(num3 > 0 ? "{0}:{1:00}:{2:00}" : "{1}:{2:00}", num3, num4, num1);
        }
    }
}