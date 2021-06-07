using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public class Episode
    {
        public string EpisodeID { get; set; }
        public string SeasonID { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public string EpisodeURL { get; set; }
    }
}
