using Newtonsoft.Json;

namespace Point.Contracts.Models
{
    public class PointVotes
    {
        public long PostId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Vote { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Likes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Dislikes { get; set; }
    }
}
