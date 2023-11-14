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

        [JsonProperty("AreaKills")]
        public int AreaKills { get; set; }

        [JsonProperty("TotalKills")]
        public int TotalKills { get; set; }
    }
}