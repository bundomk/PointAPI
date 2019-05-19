using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class VotePost
    {
        public long UserId { get; set; }
        public long InfoPostId { get; set; }
        public bool Vote { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual InfoPost InfoPost { get; set; }
        public virtual User User { get; set; }
    }
}
