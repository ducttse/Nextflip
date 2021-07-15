using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.notification
{
    public class Notification
    {
        public int notificationID { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public DateTime publishedDate { get; set; }
        public string content { get; set; }

    }
}
