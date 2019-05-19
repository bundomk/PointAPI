using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class InfoPost
    {
        public InfoPost()
        {
            Image = new HashSet<Image>();
            VotePost = new HashSet<VotePost>();
        }

        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public long? FixedBy { get; set; }
        public long? BelongTo { get; set; }
        public DateTime? FixedTime { get; set; }

        public virtual ICollection<Image> Image { get; set; }
        public virtual ICollection<VotePost> VotePost { get; set; }
        public virtual Institution BelongToNavigation { get; set; }
        public virtual Institution FixedByNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
