using Newtonsoft.Json;
using System;

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