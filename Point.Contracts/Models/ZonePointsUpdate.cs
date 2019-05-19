using System.Collections.Generic;

namespace Point.Contracts.Models
{
    public class ZonePointsUpdate
    {
        public long ZoneId { get; set; }
        public List<ZonePoint> Points { get; set; }
    }
}
