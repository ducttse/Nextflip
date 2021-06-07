using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.season;
using Nextflip.Models.episode;

namespace Nextflip.APIControllers
{
    public class ViewMediaDetails : ControllerBase
    {
        [Route("api/ViewMediaDetails/{mediaID}")]
        public JsonResult GetSeasons([FromServices] ISeasonService seasonService,string mediaID)
        {
            IEnumerable<SeasonDTO> seasons = seasonService.GetSeasonsByMediaID(mediaID);
            return new JsonResult(seasons);
        }

        [Route("api/ViewMediaDetails/{seasonID}")]
        public JsonResult GetEpisodes([FromServices] IEpisodeService episodeService,string seasonID)
        { 
            IEnumerable<EpisodeDTO> episodes = episodeService.GetEpisodesBySeasonID(seasonID);
            return new JsonResult(episodes);
        }

        
    }
}
