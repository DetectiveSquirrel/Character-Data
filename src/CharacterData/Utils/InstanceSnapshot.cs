﻿// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.InstanceSnapshot
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using CharacterData.Long_Stuff;
using PoeHUD.Models;

namespace CharacterData.Utils
{
    public class InstanceSnapshot
    {
        public InstanceSnapshot()
        {
            JoinExperience = LocalPlayer.Experience;
            JoinLevel = LocalPlayer.Level;
            JoinName = LocalPlayer.Name;
            JoinTime = DateTime.Now;
            JoinArea = LocalPlayer.Area;
            JoinAreaHash = LocalPlayer.AreaHash;
        }

        public long JoinExperience { get; set; }

        public long JoinLevel { get; set; }

        public string JoinName { get; set; }

        public DateTime JoinTime { get; set; }

        public AreaInstance JoinArea { get; set; }

        public uint JoinAreaHash { get; set; }

        public double Progress()
        {
            if (LocalPlayer.Level != 100)
                return (LocalPlayer.Experience - (double) PlayerExperience.TotalExperience[LocalPlayer.Level]) / PlayerExperience.NextExperience[LocalPlayer.Level] * 100.0;
            return 0.0;
        }

        public long ExperienceGained()
        {
            return LocalPlayer.Experience - JoinExperience;
        }

        public string LevelPercentGained()
        {
            return (LocalPlayer.Level == 100
                ? 0.0
                : ExperienceGained() * 100.0 / PlayerExperience.NextExperience[LocalPlayer.Level]).ToString("N2");
        }

        public string RunsToNextLevel()
        {
            var d = Math.Round(
                (100.0 - Progress()) / 100.0 * PlayerExperience.NextExperience[LocalPlayer.Level] / ExperienceGained(),
                0, MidpointRounding.AwayFromZero);
            if (!double.IsInfinity(d))
                return d.ToString("N0");
            return "∞";
        }

        public string TotalRunsToNextLevel()
        {
            var d = Math.Round(PlayerExperience.NextExperience[LocalPlayer.Level] / (double) ExperienceGained(), 2,
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