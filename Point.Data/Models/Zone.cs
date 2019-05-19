using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class Zone
    {
        public Zone()
        {
            ZonePoint = new HashSet<ZonePoint>();
        }

        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long InstitutionId { get; set; }

        public virtual ICollection<ZonePoint> ZonePoint { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
