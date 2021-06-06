using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.subtitle
{
    public class SubtitleDTO
    {
        public string SubtitleID { get; set; }
        public string EpisodeID { get; set; }
        public string Language { get; set; }
        public string Status { get; set; }
        public string SubtitleURL { get; set; }

    }
}
