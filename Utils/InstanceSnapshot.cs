using CharacterData.ExperienceTable;
using ExileCore;
using System;

namespace CharacterData.Utils;

public class InstanceSnapshot
{
    public InstanceSnapshot()
    {
        JoinExperience = CharacterData.LocalPlayer.Experience;
        JoinLevel = CharacterData.LocalPlayer.Level;
        JoinName = CharacterData.LocalPlayer.Name;
        JoinTime = DateTime.Now;
        JoinArea = CharacterData.LocalPlayer.Area;
        JoinAreaHash = CharacterData.LocalPlayer.AreaHash;
        JoinKills = CharacterData.LocalPlayer.Kills;
    }

    public long JoinExperience { get; set; }

    public long JoinLevel { get; set; }

    public string JoinName { get; set; }

    public DateTime JoinTime { get; set; }

    public AreaInstance JoinArea { get; set; }

    public uint JoinAreaHash { get; set; }

    public int JoinKills { get; set; }

    public static double Progress() => CharacterData.LocalPlayer.Level != 100
        ? (CharacterData.LocalPlayer.Experience - (double)PlayerExperience.TotalExperience[CharacterData.LocalPlayer.Level]) / PlayerExperience.NextExperience[CharacterData.LocalPlayer.Level] * 100.0
        : 0.0;

    public long ExperienceGained() => CharacterData.LocalPlayer.Experience - JoinExperience;

    public long KillsInArea() => CharacterData.LocalPlayer.Kills - JoinKills;

    public string LevelPercentGained() => (CharacterData.LocalPlayer.Level == 100
        ? 0.0
        : ExperienceGained() * 100.0 / PlayerExperience.NextExperience[CharacterData.LocalPlayer.Level]).ToString("N2");

    public string RunsToNextLevel()
    {
        var d = Math.Round((100.0 - Progress()) / 100.0 * PlayerExperience.NextExperience[CharacterData.LocalPlayer.Level] / ExperienceGained(), 0, MidpointRounding.AwayFromZero);
        return !double.IsInfinity(d)
            ? d.ToString("N0")
            : "∞";
    }

    public string TotalRunsToNextLevel()
    {
        var d = Math.Round(PlayerExperience.NextExperience[CharacterData.LocalPlayer.Level] / (double)ExperienceGained(), 2, MidpointRounding.AwayFromZero);
        return !double.IsInfinity(d)
            ? d.ToString("N0")
            : "∞";
    }

    public string RunTime()
    {
        var totalSeconds = (int)(DateTime.Now - JoinTime).TotalSeconds;
        var num1 = totalSeconds % 60;
        var num2 = totalSeconds / 60;
        var num3 = num2 / 60;
        var num4 = num2 % 60;
        return string.Format(num3 > 0
                ? "{0}:{1:00}:{2:00}"
                : "{1}:{2:00}",
            num3,
            num4,
            num1);
    }
}