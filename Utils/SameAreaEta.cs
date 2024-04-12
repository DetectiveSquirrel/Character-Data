using Newtonsoft.Json;

namespace CharacterData.Utils;

public class SameAreaEta
{
    [JsonProperty("Left")]
    public string Left { get; set; }

    [JsonProperty("TotalForLevel")]
    public string TotalForLevel { get; set; }
}