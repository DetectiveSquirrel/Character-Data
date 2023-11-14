using Newtonsoft.Json;
using System.Collections.Generic;

namespace CharacterData.Utils
{
    public class LevelLogs
    {
        [JsonProperty("Entries")]
        public List<Entry> Entries { get; set; }
    }
}