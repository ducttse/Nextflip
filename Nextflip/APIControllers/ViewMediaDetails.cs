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
using Nextflip.Models.category;

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
                                                [FromServices] ICategoryService categoryService,
                                                string mediaID)
        {
            try
            {
                Media media = mediaService.GetMediaByID(mediaID);
                IEnumerable<Season> seasons = seasonService.GetSeasonsByMediaID(mediaID);
                IEnumerable<Category> categories = categoryService.GetCategoriesByMediaID(mediaID);
                var episodesMap = new Dictionary<string, IEnumerable>();
                if (seasons != null)
                {
                    foreach (var season in seasons)
                    {
                        IEnumerable<Episode> episodes = episodeService.GetEpisodesBySeasonID(season.SeasonID);
                        episodesMap.Add(season.SeasonID, episodes);
                    }
                }
                var mediaDetails = new
                {
                    Media = media,
                    Categories = categories,
                    Seasons = seasons,
                    EpisodesMapSeason = episodesMap
                };
                return new JsonResult(mediaDetails);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaDetails: " + ex.Message);
                return new JsonResult("error occur");
            }

        }

        [Route("GetEpisode/{mediaID}/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] IEpisodeService episodeService,
                                                [FromServices] ISeasonService seasonService, string mediaID, string episodeID)
        {
            try
            {
                Episode episode = episodeService.GetEpisodeByID(episodeID);
                var episodeHasMediaID = new
                    {
                        Episode = episode,
                        MediaID = mediaID                    
                    };
                return new JsonResult(episodeHasMediaID);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodeByID: " + ex.Message);
                return new JsonResult("error occur");
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
                return new JsonResult("error occur");
            }
        }



    }
}
