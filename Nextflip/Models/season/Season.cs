using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.season
{
    public class Season
    {
        public string SeasonID { get; set; }
        public string Title { get; set; }
        public string ThumbnailURL { get; set; }
        public string MediaID { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }

        public bool EqualWithoutStatus(Season anotherSeason)
        {
            if (anotherSeason == null) return false;
            return Title == anotherSeason.Title &&
                   ThumbnailURL == anotherSeason.ThumbnailURL &&
                   MediaID == anotherSeason.MediaID &&
                   Number == anotherSeason.Number;
        }
    }
}
