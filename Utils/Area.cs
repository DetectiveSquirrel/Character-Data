using Newtonsoft.Json;

namespace CharacterData.Utils;

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