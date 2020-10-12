// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.Info
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using Newtonsoft.Json;

namespace CharacterData.Utils
{
  public class Info
  {
    [JsonProperty("Date")]
    public DateTime Date { get; set; }

    [JsonProperty("Level")]
    public Level Level { get; set; }

    [JsonProperty("Area")]
    public Area Area { get; set; }
  }
}
