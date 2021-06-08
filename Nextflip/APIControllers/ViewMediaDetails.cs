using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.season;
using Nextflip.Models.episode;
using Nextflip.Models.subtitle;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMediaDetails : ControllerBase
    {
        [Route("GetSeasons/{mediaID}")]
        public IActionResult GetSeasons([FromServices] ISeasonService seasonService,string mediaID)
        {
            IEnumerable<Season> seasons = seasonService.GetSeasonsByMediaID(mediaID);
            return new JsonResult(seasons);
        }

        [Route("GetEpisodes/{seasonID}")]
        public IActionResult GetEpisodes([FromServices] IEpisodeService episodeService,string seasonID)
        { 
            IEnumerable<Episode> episodes = episodeService.GetEpisodesBySeasonID(seasonID);
            return new JsonResult(episodes);
        }

        [Route("GetEpisode/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] IEpisodeService episodeService, string episodeID)
        {
            Episode episode = episodeService.GetEpisodeByID(episodeID);
            return new JsonResult(episode);
        }

        [Route("GetSubtitle/{subtitleID}")]
        public IActionResult GetSubtitleByID([FromServices] ISubtitleService subtitleService, string subtitleID)
        {
            Subtitle subtitle = subtitleService.GetSubtitleByID(subtitleID);
            return new JsonResult(subtitle);
        }

    }
}
