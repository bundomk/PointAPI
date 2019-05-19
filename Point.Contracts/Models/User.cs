using System;

namespace Point.Contracts.Models
{
    public class User
    {
        public long Id { get; set; }
        public Guid Key { get; set; }
        public string DeviceId { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
