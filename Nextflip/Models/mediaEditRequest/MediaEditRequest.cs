using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaEditRequest
{
    public class MediaEditRequest
    {
        public int requestID { get; set; }
        public string userEmail { get; set; }
        public string mediaID { get; set; }
        public string status { get; set; }
        public string note { get; set; }
        public string type { get; set; }
        public string ID { get; set; }
        public string mediaTitle { get; set; }
    }
}
