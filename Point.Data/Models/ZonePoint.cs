using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class ZonePoint
    {
        public long Id { get; set; }
        public long ZoneId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public virtual Zone Zone { get; set; }
    }
}
