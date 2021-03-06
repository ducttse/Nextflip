using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models;
using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.season;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewEditorDashboard : ControllerBase
    {
        private readonly ILogger _logger;

        public ViewEditorDashboard(ILogger<ViewEditorDashboard> logger)
        {
            _logger = logger;
        }

        public class Request
        {
            public string SearchValue { get; set; }
            public string MediaID { get; set; }
            public string UserEmail { get; set; }
            public string Note { get; set; }
            public int CategoryID { get; set; }
            public string Status { get; set; }
            public string CategoryName { get; set; }
            public string ID { get; set; }
            public string Type { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
            public string Title { get; set; }
            public string FilmType { get; set; }
            public string Director { get; set; }
            public string Cast { get; set; }
            public int? PublishYear { get; set; }
            public string Duration { get; set; }
            public string BannerURL { get; set; }
            public string Language { get; set; }
            public string Description { get; set; }
            public string[] CategoryIDArray { get; set; }

        }


        [Route("GetCategories")]
        public IActionResult GetCategories([FromServices] ISubscribedUserService subscribedUserService)
        {
            try
            {
                IEnumerable<Category> categories = subscribedUserService.GetCategories();
                return new JsonResult(categories);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetCategories: " + ex.Message);
                return new JsonResult("error occur");
            }
        }

        [HttpPost]
        [Route("RequestDisableMedia")]
        public IActionResult RequestDisableMedia([FromServices] IEditorService editorService, [FromForm] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                bool requestDisableMedia = editorService.RequestDisableMedia(request.MediaID);
                //bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, request.MediaID, request.Note, "", request.MediaID);
                if (!requestDisableMedia) return new JsonResult(messageFail);
                var message = new
                {
                    message = "success"
                };
                return (new JsonResult(message));
            }
            catch (Exception e)
            {
                _logger.LogInformation("RequestDisabledMedia: " + e.Message);
                var result = new
                {
                    message = e.Message
                };
                return new JsonResult(result);
            }
        }

        [HttpPost]
        [Route("RequestChangeMediaStatus")]
        public IActionResult RequestChangeMediaStatus([FromServices] IEditorService editorService, [FromForm] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                if (request.Status.ToLower().Trim().Equals(editorService.GetMediaByID(request.ID).Status.ToLower()))
                    return new JsonResult(new { messageFail = "fail. A wrong process." });
                bool requestChange;
                if (request.Status.ToLower().Trim().Equals("approved"))
                    requestChange = editorService.RequestChangeMediaStatus(request.ID, request.Status, null);
                else 
                    requestChange = editorService.RequestChangeMediaStatus(request.ID, request.Status, request.Note);
                if (!requestChange) return new JsonResult(messageFail);
                var message = new
                {
                    message = "success"
                };
                return (new JsonResult(message));
            }
            catch (Exception e)
            {
                _logger.LogInformation("RequestChangeMediaStatus: " + e.Message);
                var result = new
                {
                    message = e.Message
                };
                return new JsonResult(result);
            }
        }

        //Get
        [HttpPost]
        [Route("GetMedia")]
        public JsonResult GetMedia([FromServices] IEditorService editorService,
                                [FromBody] Request request)
        {
            try
            {
                if (request.Status.Trim() == "") request.Status = "all";
                if (request.CategoryName.Trim() == "") request.CategoryName = "all";
                IEnumerable<Media> mediaList = editorService.ViewMediasFilterCategory_Status(request.CategoryName.Trim().ToLower(),
                    request.Status.Trim(), request.RowsOnPage, request.RequestPage);
                List<MediaShow> mediaShowList = new List<MediaShow>();
                foreach (var item in mediaList)
                {
                    mediaShowList.Add(new MediaShow
                    {
                        MediaID = item.MediaID,
                        Status = item.Status,
                        Title = item.Title,
                        FilmType = item.FilmType,
                        Director = item.Director,
                        Cast = item.Cast,
                        PublishYear = item.PublishYear,
                        Duration = item.Duration,
                        BannerURL = item.BannerURL,
                        Language = item.Language,
                        Description = item.Description,
                        UploadDate = item.UploadDate,
                        CountSeason = editorService.NumberAvailableSeason(item.MediaID),
                        Note = item.Note
                    });
                }
                int count = editorService.NumberOfMediasFilterCategory_Status(request.CategoryName.Trim().ToLower(), request.Status.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalMedia = count,
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = mediaShowList
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaRequest: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        //Search
        [Route("SearchingMedia")]
        public JsonResult SearchingMedia([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                if (request.Status.Trim() == "") request.Status = "all";
                if (request.CategoryName.Trim() == "") request.CategoryName = "all";
                IEnumerable<Media> mediaList = editorService.GetMediasByTitleFilterCategory_Status(request.SearchValue.Trim().ToLower(),
                    request.CategoryName.Trim().ToLower(), request.Status.Trim(), request.RowsOnPage, request.RequestPage);
                List<MediaShow> mediaShowList = new List<MediaShow>();
                foreach (var item in mediaList)
                {
                    mediaShowList.Add(new MediaShow
                    {
                        MediaID = item.MediaID,
                        Status = item.Status,
                        Title = item.Title,
                        FilmType = item.FilmType,
                        Director = item.Director,
                        Cast = item.Cast,
                        PublishYear = item.PublishYear,
                        Duration = item.Duration,
                        BannerURL = item.BannerURL,
                        Language = item.Language,
                        Description = item.Description,
                        UploadDate = item.UploadDate,
                        CountSeason = editorService.NumberAvailableSeason(item.MediaID),
                        Note = item.Note
                    });
                }
                int count = editorService.NumberOfMediasBySearchingFilterCategory_Status(request.SearchValue.Trim().ToLower(),
                    request.CategoryName.Trim().ToLower(), request.Status.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalMedia = count,
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = mediaShowList
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SearchingMediaRequest: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("AddMedia")]
        public IActionResult AddMedia([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                if (request.Title.Trim() == "" || request.Description.Trim() == "" ||
                    request.BannerURL.Trim() == "" || request.CategoryIDArray == null)
                    return new JsonResult(messageFail);
                int[] myInts = Array.ConvertAll(request.CategoryIDArray, s => int.Parse(s));
                string mediaID = editorService.AddMedia(request.Title, request.FilmType, request.Director,
                    request.Cast, request.PublishYear, request.Duration, request.BannerURL, request.Language, request.Description);
                if (mediaID == null) return new JsonResult(messageFail);
                //bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, mediaID, "Request add media", "media", mediaID);
                //if (!addMediaRequest) return new JsonResult(messageFail);
                for (int i = 0; i < myInts.Length; i++)
                {
                    bool addMediaCategory = editorService.AddMediaCategory(mediaID, myInts[i]);
                    if (!addMediaCategory) return new JsonResult(messageFail);
                }
                var message = new
                {
                    message = "success"
                };
                return (new JsonResult(message));
            }
            catch (Exception e)
            {
                _logger.LogInformation("AddMedia: " + e.Message);
                var result = new
                {
                    message = e.Message
                };
                return new JsonResult(result);
            }
        }
        public class MediaForm
        {
            public string MediaID { get; set; }
            public string Status { get; set; }
            public string Title { get; set; }
            public string FilmType { get; set; }
            public string Director { get; set; }
            public string Cast { get; set; }
            public string PublishYear { get; set; }
            public string Duration { get; set; }
            public string BannerURL { get; set; }
            public string Language { get; set; }
            public string Description { get; set; }
            public int[] CategoryIDArray { get; set; }
            public string UserEmail { get; set; }
        }

        [Route("GetMediaByID/{mediaID}")]
        public IActionResult GetMediaByID([FromServices] ISubscribedUserService subscribedUserService, string mediaID)
        {
            try
            {
                Media media = subscribedUserService.GetMediaByID(mediaID);
                IEnumerable<Category> categories = subscribedUserService.GetCategoriesByMediaID(mediaID);
                var mediaDetail = new
                {
                    Media = media,
                    Category = categories
                };
                return new JsonResult(mediaDetail);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaByID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }


        public class SeasonForm
        {
            public string SeasonID { get; set; }
            public string Title { get; set; }
            public string ThumbnailURL { get; set; }
            public string MediaID { get; set; }
            public string Status { get; set; }
            public string Number { get; set; }
            public string UserEmail { get; set; }
        }
        [Route("AddSeason")]
        [HttpPost]
        public IActionResult AddSeason([FromServices] IEditorService editorService, [FromBody] SeasonForm frmSeason)
        {
            try
            {
                int number;
                bool isValidInfo = int.TryParse(frmSeason.Number, out number) && frmSeason.MediaID != null && frmSeason.Title.Trim() != string.Empty
                                    && frmSeason.ThumbnailURL.Trim() != string.Empty && frmSeason.UserEmail != null ;
                if( isValidInfo == false) return new JsonResult(new { Message = "Fail" });
                Season season = new Season
                {
                    MediaID = frmSeason.MediaID,
                    Title = frmSeason.Title,
                    ThumbnailURL = frmSeason.ThumbnailURL,
                    Number = number
                };
                string seasonID = editorService.AddSeason(season);
                if (seasonID == null) return new JsonResult(new { Message = "Fail" });
                //bool addMediaRequest = editorService.AddMediaRequest(frmSeason.UserEmail, frmSeason.MediaID, "add new season", "season", seasonID);
                //if(addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
                return new JsonResult(new { Message = "Success" , SeasonID = seasonID });
            }
            catch (Exception e)
            {
                _logger.LogInformation("AddSeason: " + e.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + e.Message
                }) ;
            }
        }
        [Route("GetSeasonByID/{seasonID}")]
        public IActionResult GetSeasonByID([FromServices] ISubscribedUserService subscribedUserService, string seasonID)
        {
            try
            {
                Season season = subscribedUserService.GetSeasonByID(seasonID);
                return new JsonResult(season);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetSeasonByID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
        [Route("EditSeason")]
        [HttpPost]
        public IActionResult EditSeason([FromServices] IEditorService editorService, [FromBody] SeasonForm frmSeason)
        {
            try
            {
                int number;
                bool isValidInfo = int.TryParse(frmSeason.Number, out number) && frmSeason.SeasonID != null && frmSeason.MediaID != null && frmSeason.Title.Trim() != string.Empty
                                    && frmSeason.ThumbnailURL.Trim() != string.Empty && frmSeason.UserEmail != null;
                if (isValidInfo == false) return new JsonResult(new { Message = "Fail" });
                Season season = new Season
                {
                    SeasonID = frmSeason.SeasonID,
                    Title = frmSeason.Title,
                    ThumbnailURL = frmSeason.ThumbnailURL,
                    Number = number
                };
                string seasonID = editorService.UpdateSeason(season);
                if (seasonID == null) return new JsonResult(new { Message = "Fail" });
                //bool addMediaRequest = editorService.AddMediaRequest(frmSeason.UserEmail, frmSeason.MediaID, "update the season", "season", seasonID);
                //if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
                return new JsonResult(new { Message = "Success"});
            }
            catch (Exception e)
            {
                _logger.LogInformation("EditSeason: " + e.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + e.Message
                });
            }
        }

        public class EpisodeForm
        {
            public string EpisodeID { get; set; }
            public string Title { get; set; }
            public string ThumbnailURL { get; set; }
            public string SeasonID { get; set; }
            public string Status { get; set; }
            public string Number { get; set; }
            public string EpisodeURL { get; set; }
            public string UserEmail { get; set; }
        }

        [Route("AddEpisode")]
        [HttpPost]
        public IActionResult AddEpisode([FromServices] IEditorService editorService, [FromBody] EpisodeForm frmEpisode)
        {
            try
            {
                int number;
                bool isValidInfo = int.TryParse(frmEpisode.Number, out number) && frmEpisode.SeasonID != null && frmEpisode.Title.Trim() != string.Empty
                                    && frmEpisode.ThumbnailURL.Trim() != string.Empty && frmEpisode.EpisodeURL.Trim() != string.Empty  && frmEpisode.UserEmail != null;
                if (isValidInfo == false) return new JsonResult(new { Message = "Fail" });
                Episode episode = new Episode
                {
                    SeasonID = frmEpisode.SeasonID,
                    Title = frmEpisode.Title,
                    ThumbnailURL = frmEpisode.ThumbnailURL,
                    EpisodeURL = frmEpisode.EpisodeURL,
                    Number = number
                };
                string episodeID = editorService.AddEpisode(episode);
                if (episodeID == null ) return new JsonResult(new { Message = "Fail" });
                string type = "episode";
                Media media = editorService.GetMediaByChildID(episodeID, type);
                if (media == null) return new JsonResult(new { Message = "Fail" });
                //bool addMediaRequest = editorService.AddMediaRequest(frmEpisode.UserEmail, media.MediaID, "add new episode", type, episodeID);
                //if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
                return new JsonResult(new { Message = "Success", EpisodeID = episodeID });
            }
            catch (Exception e)
            {
                _logger.LogInformation("Episode: " + e.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + e.Message
                });
            }
        }
        [Route("GetEpisodeByID/{episodeID}")]
        public IActionResult GetEpisodeByID([FromServices] ISubscribedUserService subscribedUserService, string episodeID)
        {
            try
            {
                Episode episode = subscribedUserService.GetEpisodeByID(episodeID);
                return new JsonResult(episode);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodeByID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
        [Route("EditEpisode")]
        [HttpPost]
        public IActionResult EditEpisode([FromServices] IEditorService editorService, [FromBody] EpisodeForm frmEpisode)
        {
            try
            {
                int number;
                bool isValidInfo = int.TryParse(frmEpisode.Number, out number) && frmEpisode.EpisodeID != null && frmEpisode.Title.Trim() != string.Empty
                                    && frmEpisode.ThumbnailURL.Trim() != string.Empty && frmEpisode.EpisodeURL.Trim() != string.Empty && frmEpisode.UserEmail != null;
                if (isValidInfo == false) return new JsonResult(new { Message = "Fail" });
                Episode episode = new Episode
                {
                    EpisodeID = frmEpisode.EpisodeID,
                    Title = frmEpisode.Title,
                    ThumbnailURL = frmEpisode.ThumbnailURL,
                    EpisodeURL = frmEpisode.EpisodeURL,
                    Number = number
                };
                string episodeID = editorService.UpdateEpisode(episode);
                if (episodeID == null) return new JsonResult(new { Message = "Fail" });
                string type = "episode";
                Media media = editorService.GetMediaByChildID(episodeID, type);
                if (media == null) return new JsonResult(new { Message = "Fail" });
                //bool addMediaRequest = editorService.AddMediaRequest(frmEpisode.UserEmail, media.MediaID, "update episode", type, episodeID);
                //if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
                return new JsonResult(new { Message = "Success", EpisodeID = episodeID });
            }
            catch (Exception e)
            {
                _logger.LogInformation("EditEpisode: " + e.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + e.Message
                });
            }
        }

        [Route("AddNewMedia")]
        [HttpPost]
        public IActionResult AddNewMedia([FromServices] IEditorService editorService, [FromBody] PrototypeMediaForm mediaForm)
        {
            try
            {
                var newMediaID = editorService.AddNewMedia(mediaForm);
                return new JsonResult(new {
                    Message = "Success", MediaID = newMediaID
                });
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Add New Media: " + exception.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + exception.Message
                });
            }
        }

        public class PrototypeSeasonForm
        {
            public Season SeasonInfo { set; get; }
            public IEnumerable<Episode> Episodes { get; set; }

            public PrototypeSeasonForm(Season seasonInfo, IEnumerable<Episode> episodes)
            {
                SeasonInfo = seasonInfo;
                Episodes = episodes;
            }
        }

        public class PrototypeMediaForm {
            public Media MediaInfo { get; set; }
            public IEnumerable<PrototypeSeasonForm> Seasons { get; set; }

            public IEnumerable<int> CategoryIDs { get; set; }
        }

        [Route("EditMedia")]
        [HttpPost]
        public IActionResult Edit([FromServices] IEditorService editorService, [FromBody] PrototypeMediaForm mediaForm)
        {
            try
            {
                var mediaID = editorService.EditMedia(mediaForm);
                return new JsonResult(new
                {
                    Message = "Success",
                    MediaID = mediaID
                });
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Edit Media: " + exception.Message);
                return new JsonResult(new
                {
                    Message = "Fail." + exception.Message
                });
            }
        }
        
        public class MediaShow
        {
            public string MediaID { get; set; }
            public string Status { get; set;}
            public string Title { get; set; }
            public string FilmType { get; set; }
            public string Director { get; set; }
            public string Cast { get; set; }
            public int? PublishYear { get; set; }
            public string Duration { get; set; }
            public string BannerURL { get; set; }
            public string Language { get; set; }
            public string Description { get; set; }
            public DateTime UploadDate { get; set; }
            public int CountSeason { get; set; }
            public string Note { get; set; }
        }
    }
}

