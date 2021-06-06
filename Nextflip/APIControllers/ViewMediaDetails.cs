using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.season;
using Nextflip.Models.episode;

namespace Nextflip.APIControllers
{
    public class ViewMediaDetails : ControllerBase
    {

        public JsonResult GetSeasons(ISeasonService seasonService,[FromForm] string mediaID)
        {
            IEnumerable<SeasonDTO> seasons = seasonService.GetSeasonsByMediaID(mediaID);
            return new JsonResult(seasons);
        }

        public JsonResult GetEpisodes(IEpisodeService episodeService,[FromForm] string seasonID)
        { 
            IEnumerable<EpisodeDTO> episodes = episodeService.GetEpisodesBySeasonID(seasonID);
            return new JsonResult(episodes);
        }

        
    }
}
