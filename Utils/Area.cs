// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.Area
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using Newtonsoft.Json;

namespace CharacterData.Utils
{
    public class Area
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("LevelDifference")]
        public string LevelDifference { get; set; }

        [JsonProperty("TimeSpent")]
        public string TimeSpent { get; set; }

        [JsonProperty("SameAreaEta")]
        public SameAreaEta SameAreaEta { get; set; }
    }
}
