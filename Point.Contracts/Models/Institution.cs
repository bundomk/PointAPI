using System;
using System.Collections.Generic;

namespace Point.Contracts.Models
{
    public class Institution
    {
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
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public List<Zone> Zones { get; set; }
    }
}
