using System;
using System.Collections.Generic;

namespace Point.Contracts.Models
{
    public class Zone
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long InstitutionId { get; set; }
        public List<ZonePoint> Points { get; set; }
    }
}
