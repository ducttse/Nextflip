using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.season;
using Nextflip.Models.episode;
using Nextflip.Models.subtitle;
using Nextflip.Models.media;
using System.Collections;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMediaDetails : ControllerBase
    {
        [Route ("GetMediaDetails/{mediaID}")]
        public IActionResult GetMediaDetails([FromServices] IMediaService mediaService,
                                                [FromServices] ISeasonService seasonService,
                                                [FromServices] IEpisodeService episodeService,
                                                string mediaID)
        {
            Media media = mediaService.GetMediaByID(mediaID);
            IEnumerable<Season> seasons = seasonService.GetSeasonsByMediaID(mediaID);
            var episodesMap = new Dictionary<string, IEnumerable>();
            foreach(var season in seasons)
            {
                IEnumerable<Episode> episodes = episodeService.GetEpisodesBySeasonID(season.SeasonID);
                episodesMap.Add(season.SeasonID, episodes);
            }
            var mediaDetails = new {
                    Media = media,
                    Seasons = seasons,
                    EpisodesMapSeason = episodesMap
            };
            return new JsonResult(mediaDetails);
        }
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
