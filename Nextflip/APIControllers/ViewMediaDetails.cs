using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.season;
using Nextflip.Models.episode;
using Nextflip.Models.subtitle;
using Nextflip.Models.media;
using System.Collections;
using Microsoft.Extensions.Logging;
using System;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMediaDetails : ControllerBase
    {
        private readonly ILogger _logger;

        public ViewMediaDetails(ILogger<ViewSubscribedUserDashboard> logger)
        {
            _logger = logger;
        }
        [Route ("GetMediaDetails/{mediaID}")]
        public IActionResult GetMediaDetails([FromServices] IMediaService mediaService,
                                                [FromServices] ISeasonService seasonService,
                                                [FromServices] IEpisodeService episodeService,
                                                string mediaID)
        {
            try
            {
                Media media = mediaService.GetMediaByID(mediaID);
                IEnumerable<Season> seasons = seasonService.GetSeasonsByMediaID(mediaID);
                var episodesMap = new Dictionary<string, IEnumerable>();
                foreach (var season in seasons)
                {
                    IEnumerable<Episode> episodes = episodeService.GetEpisodesBySeasonID(season.SeasonID);
                    episodesMap.Add(season.SeasonID, episodes);
                }
                var mediaDetails = new
                {
                    Media = media,
                    Seasons = seasons,
                    EpisodesMapSeason = episodesMap
                };
                return new JsonResult(mediaDetails);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaDetails: " + ex.Message);
                return null;
            }

        }
        [Route("GetSeasons/{mediaID}")]
        public IActionResult GetSeasons([FromServices] ISeasonService seasonService,string mediaID)
        {
            try
            {
                IEnumerable<Season> seasons = seasonService.GetSeasonsByMediaID(mediaID);
                return new JsonResult(seasons);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSeasons: " + ex.Message);
                return null;
            }

        }

        [Route("GetEpisodes/{seasonID}")]
        public IActionResult GetEpisodes([FromServices] IEpisodeService episodeService,string seasonID)
        {
            try
            {
                IEnumerable<Episode> episodes = episodeService.GetEpisodesBySeasonID(seasonID);
                return new JsonResult(episodes);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodes: " + ex.Message);
                return null;
            }
        }

        [Route("GetEpisode/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] IEpisodeService episodeService, string episodeID)
        {
            try
            {
                Episode episode = episodeService.GetEpisodeByID(episodeID);
                return new JsonResult(episode);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodeByID: " + ex.Message);
                return null;
            }
        }

        [Route("GetSubtitle/{subtitleID}")]
        public IActionResult GetSubtitleByID([FromServices] ISubtitleService subtitleService, string subtitleID)
        {
            try
            {
                Subtitle subtitle = subtitleService.GetSubtitleByID(subtitleID);
                return new JsonResult(subtitle);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSubtitleByID: " + ex.Message);
                return null;
            }
        }



    }
}
