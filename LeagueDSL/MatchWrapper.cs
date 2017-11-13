using System.Collections.Generic;
using LeagueModels.MatchEndpoint;
using Newtonsoft.Json;

namespace League {
    public class MatchWrapper {

        [JsonProperty("matches")]
        public List<Match> matches { get; set; }
    }
}