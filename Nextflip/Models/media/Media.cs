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

        public bool EqualWithoutStatus(Media anotherMedia)
        {
            if (anotherMedia is null) return false;
            return Title == anotherMedia.Title &&
                   FilmType == anotherMedia.FilmType &&
                   Director == anotherMedia.Director &&
                   Cast == anotherMedia.Cast &&
                   PublishYear == anotherMedia.PublishYear &&
                   Duration == anotherMedia.Duration &&
                   BannerURL == anotherMedia.BannerURL &&
                   Language == anotherMedia.Language &&
                   Description == anotherMedia.Description;
        }
    }
}
