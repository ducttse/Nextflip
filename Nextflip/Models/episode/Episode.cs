using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public class Episode
    {
        public string EpisodeID { get; set; }
        public string Title { get; set; }
        public string ThumbnailURL { get; set; }
        public string SeasonID { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public string EpisodeURL { get; set; }

        public bool EqualWithoutStatus (Episode anotherEpisode)
        {
            if (anotherEpisode == null) return false;
            return Title == anotherEpisode.Title &&
                   ThumbnailURL == anotherEpisode.ThumbnailURL &&
                   SeasonID == anotherEpisode.SeasonID &&
                   Number == anotherEpisode.Number &&
                   EpisodeURL == anotherEpisode.EpisodeURL;

        }
    }
}
