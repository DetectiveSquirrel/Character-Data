using Newtonsoft.Json;

namespace CharacterData.Utils
{
    public class Entry
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Info")]
        public Info Info { get; set; }
    }
}