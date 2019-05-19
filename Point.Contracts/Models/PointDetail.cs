using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Point.Contracts.Models
{
    public class PointDetail
    {
        public long Id { get; set; }
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid? RequestId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<string> ImageUrls { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ApprovedTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BelongToName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreationTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FixedByName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FixedTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsApproved { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Vote { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Likes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Dislikes { get; set; }
    }
}
