﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetMediaDetails([FromServices] ISubscribedUserService subscribedUserService, string mediaID)
        {
            try
            {
                Media media = subscribedUserService.GetMediaByID(mediaID);
                IEnumerable<Season> seasons = subscribedUserService.GetSeasonsByMediaID(mediaID);
                IEnumerable<Category> categories = subscribedUserService.GetCategoriesByMediaID(mediaID);
                var episodesMap = new Dictionary<string, IEnumerable>();
                if (seasons != null)
                {
                    foreach (var season in seasons)
                    {
                        IEnumerable<Episode> episodes = subscribedUserService.GetEpisodesBySeasonID(season.SeasonID);
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
                return new JsonResult("error occur" + ex.Message);
            }

        }

        [Route("GetEpisode/{mediaID}/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] ISubscribedUserService subscribedUserService, string mediaID, string episodeID)
        {
            try
            {
                Episode episode = subscribedUserService.GetEpisodeByID(episodeID);
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
        public IActionResult GetSubtitleByID([FromServices] ISubscribedUserService subscribedUserService, string subtitleID)
        {
            try
            {
                Subtitle subtitle = subscribedUserService.GetSubtitleByID(subtitleID);
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
