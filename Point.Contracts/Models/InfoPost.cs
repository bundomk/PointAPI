using System;
using System.Collections.Generic;

namespace Point.Contracts.Models
{
    public class InfoPost
    {
        public string Description { get; set; }
        public long UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<byte[]> Images { get; set; }
    }
}
