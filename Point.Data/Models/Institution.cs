using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class Institution
    {
        public Institution()
        {
            InfoPostBelongToNavigation = new HashSet<InfoPost>();
            InfoPostFixedByNavigation = new HashSet<InfoPost>();
            Zone = new HashSet<Zone>();
        }

        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string ResponsiblePersonName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        public virtual ICollection<InfoPost> InfoPostBelongToNavigation { get; set; }
        public virtual ICollection<InfoPost> InfoPostFixedByNavigation { get; set; }
        public virtual ICollection<Zone> Zone { get; set; }
    }
}
