// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.SameAreaEta
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using Newtonsoft.Json;

namespace CharacterData.Utils
{
  public class SameAreaEta
  {
    [JsonProperty("Left")]
    public string Left { get; set; }

    [JsonProperty("TotalForLevel")]
    public string TotalForLevel { get; set; }
  }
}
