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
using Nextflip.Models;

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

        [Route("GetSeasons/{mediaID}")]
        public IActionResult GetSeasons([FromServices] ISubscribedUserService subscribedUserService, string mediaID)
        {
            try
            {
                IEnumerable<Season> seasons = subscribedUserService.GetSeasonsByMediaID(mediaID);
                return new JsonResult(seasons);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSeasons: " + ex.Message);
                return new JsonResult("error occur" + ex.Message);
            }
        }

        [Route("GetEpisodesOfSeason/{seasonID}")]
        public IActionResult GetEpisodesOfSeason([FromServices] ISubscribedUserService subscribedUserService, string seasonID)
        {
            try
            {
                IEnumerable<Episode> episodes = subscribedUserService.GetEpisodesBySeasonID(seasonID);
                return new JsonResult(episodes);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodesOfSeason: " + ex.Message);
                return new JsonResult("error occur" + ex.Message);
            }
        }

        public partial class EpisodeJson
        {
            public string MediaTitle { get; set; }
            public string MediaDescription { get; set; }
            public IEnumerable<Category> Categories { get; set; }
            public string EpisodeID { get; set; }
            public string Title { get; set; }
            public string ThumbnailURL { get; set; }
            public string SeasonID { get; set; }
            public string Status { get; set; }
            public int Number { get; set; }
            public string EpisodeURL { get; set; }
        }
        [Route("GetEpisode/{mediaID}/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] ISubscribedUserService subscribedUserService, string mediaID, string episodeID)
        {
            try
            {
                EpisodeJson episodeJson = null;
                Episode episode = subscribedUserService.GetEpisodeByID(episodeID);
                IEnumerable<Category> categories = subscribedUserService.GetCategoriesByMediaID(mediaID);
                Media media = subscribedUserService.GetMediaByID(mediaID);
                if (episode != null && media != null)
                {
                    episodeJson = new EpisodeJson
                    {
                        MediaTitle = media.Title,
                        MediaDescription = media.Description,
                        Categories = categories,
                        EpisodeID = episode.EpisodeID,
                        Title = episode.Title,
                        ThumbnailURL = episode.ThumbnailURL,
                        SeasonID = episode.SeasonID,
                        Status = episode.Status,
                        Number = episode.Number,
                        EpisodeURL = episode.EpisodeURL
                    };
                }
                return new JsonResult(episodeJson);
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

        public partial class MediaUserID
        {
            public string UserID { get; set; }
            public string MediaID { get; set; }
        }
        [Route("AddMediaToFavorite")]
        [HttpPost]
        public IActionResult AddMediaToFavorite([FromServices] ISubscribedUserService subscribedUserService, MediaUserID mediaUserID)
        {
            try
            {
                subscribedUserService.AddMediaToFavoriteList(mediaUserID.UserID, mediaUserID.MediaID);
                return new JsonResult(new NotificationObject { message = "Success"});
            }
            catch (Exception ex)
            {
                _logger.LogInformation("AddMediaToFavorite: " + ex.Message);
                return new JsonResult(new NotificationObject { message = "Fail" });
            }
        }

        [Route("RemoveMediaFromFavorite")]
        [HttpPost]
        public IActionResult RemoveMediaFromFavorite([FromServices] ISubscribedUserService subscribedUserService, MediaUserID mediaUserID)
        {
            try
            {
                subscribedUserService.RemoveMediaFromFavoriteList(mediaUserID.UserID, mediaUserID.MediaID);
                return new JsonResult(new NotificationObject { message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("RemoveMediaFromFavorite: " + ex.Message);
                return new JsonResult(new NotificationObject { message = "Fail" });
            }
        }

        [Route("IsFavoriteMedia")]
        [HttpPost]
        public IActionResult IsFavoriteMedia([FromServices] ISubscribedUserService subscribedUserService, MediaUserID mediaUserID)
        {
            try
            {
                bool isFavoriteMedia = subscribedUserService.IsFavoriteMedia(mediaUserID.UserID, mediaUserID.MediaID);
                return new JsonResult(new NotificationObject { message = isFavoriteMedia.ToString()});

            }
            catch (Exception ex)
            {
                _logger.LogInformation("IsFavoriteMedia: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
    }
}
