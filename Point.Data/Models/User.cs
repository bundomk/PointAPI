using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class User
    {
        public User()
        {
            InfoPost = new HashSet<InfoPost>();
            VotePost = new HashSet<VotePost>();
        }

        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<InfoPost> InfoPost { get; set; }
        public virtual ICollection<VotePost> VotePost { get; set; }
    }
}
