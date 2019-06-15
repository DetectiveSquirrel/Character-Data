// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.Level
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using Newtonsoft.Json;

namespace CharacterData.Utils
{
  public class Level
  {
    [JsonProperty("CurrentLevel")]
    public int CurrentLevel { get; set; }

    [JsonProperty("CurrentLevelPercent")]
    public double CurrentLevelPercent { get; set; }

    [JsonProperty("ExperiencedGained")]
    public long ExperiencedGained { get; set; }

    [JsonProperty("ExperienceGainedPercent")]
    public string ExperienceGainedPercent { get; set; }

    [JsonProperty("LevelUps")]
    public int LevelUps { get; set; }
  }
}
