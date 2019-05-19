using System;
using System.Collections.Generic;

namespace Point.Data.Models
{
    public partial class Image
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long InfoPostId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public DateTime? TakenTime { get; set; }
        public string CameraMaker { get; set; }
        public string CameraModel { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }

        public virtual InfoPost InfoPost { get; set; }
    }
}
