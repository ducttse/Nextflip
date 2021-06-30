using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public class Media
    {
        public string MediaID { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string FilmType { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public int? PublishYear { get; set; }
        public string Duration { get; set; }
        public string BannerURL { get; set; }
        public string Language { get; set; }
        public string Description{ get; set; }
    }
}
