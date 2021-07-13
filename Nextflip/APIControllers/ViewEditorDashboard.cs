using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models;
using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.mediaEditRequest;
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
            public int[] CategoryIDArray { get; set; }

        }


        //Search all 
        [HttpPost]
        [Route("GetMediasByTitle")]
        public JsonResult GetMediasByTitle([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Media> medias = editorService.GetMediasByTitle(request.SearchValue.Trim(), request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasBySearching(request.SearchValue.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        //Search all + filter category
        [HttpPost]
        [Route("GetMediasByTitleFilterCategory")]
        public JsonResult GetMediasByTitleFilterCategory([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Media> medias = editorService.GetMediasByTitleFilterCategory(request.SearchValue.Trim(), request.CategoryName, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasBySearchingFilterCategory(request.SearchValue.Trim(), request.CategoryName);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        //Search all + filter category + filter status 
        [HttpPost]
        [Route("GetMediasByTitleFilterCategory_Status")]
        public JsonResult GetMediasByTitleFilterCategory_Status([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Media> medias = editorService.GetMediasByTitleFilterCategory_Status(request.SearchValue.Trim(), request.CategoryName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasBySearchingFilterCategory_Status(request.SearchValue.Trim(), request.CategoryName, request.Status);
                double totalPage = (int)(double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }


        //View all filter category
        [HttpPost]
        [Route("ViewMediasFilterCategory")]
        public IActionResult ViewMediasFilterCategory([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Media> medias = editorService.GetMediaFilterCategory(request.CategoryName, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasFilterCategory(request.CategoryName);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetMedias: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        //View all filter category + status
        [HttpPost]
        [Route("ViewMediasFilterCategory_Status")]
        public IActionResult ViewMediasFilterCategory_Status([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Media> medias = editorService.ViewMediasFilterCategory_Status(request.CategoryName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasFilterCategory_Status(request.CategoryName, request.Status);
                double totalPage = (int)(double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetMedias: " + e.Message);
                return new JsonResult("An error occurred");
            }
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
                bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, request.MediaID, request.Note, "", request.MediaID);
                if (!requestDisableMedia || !addMediaRequest) return new JsonResult(messageFail);
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

        //View all
        [HttpPost]
        [Route("ViewAllMedia")]
        public IActionResult ViewAllMedia([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Media> medias = editorService.GetAllMedia(request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMedias();
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Get All Medias: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        //View all + filter Status
        [HttpPost]
        [Route("ViewAllMediaFilterStatus")]
        public IActionResult ViewAllMediaFilterStatus([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Media> medias = editorService.GetAllMediaFilterStatus(request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasFilterStatus(request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Get All Medias Filter Status: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

        //Search all + Filter Status 
        [HttpPost]
        [Route("GetMediasByTitleFilterStatus")]
        public JsonResult GetMediasByTitleFilterStatus([FromServices] IEditorService editorService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Media> medias = editorService.GetMediasByTitleFilterStatus(request.SearchValue.Trim(), request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfMediasBySearchingFilterStatus(request.SearchValue.Trim(), request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = medias
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return new JsonResult("Error occur");
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
                Media mediaByChildID = editorService.GetMediaByChildID(request.ID, request.Type);
                bool requestChange = false;
                bool addMediaRequest = false;
                request.ID = request.ID + "_" + request.Status;
                if (request.Type.Trim().Equals("media"))
                {
                    requestChange = editorService.RequestChangeMediaStatus(request.ID, request.Status);
                }
                else if (request.Type.Trim().Equals("season"))
                {
                    requestChange = editorService.RequestChangeSeasonStatus(request.ID, request.Status);
                }
                else if (request.Type.Trim().Equals("episode"))
                {
                    requestChange = editorService.RequestChangeEpisodeStatus(request.ID, request.Status);
                }
                else if (request.Type.Trim().Equals("subtitle"))
                {
                    requestChange = editorService.RequestChangeSubtitleStatus(request.ID, request.Status);
                }
                if (request.Note.Trim() == "") request.Note = "Request change status";
                addMediaRequest = editorService.AddMediaRequest(request.UserEmail, mediaByChildID.MediaID, request.Note, request.Type, request.ID);

                if (!requestChange || !addMediaRequest) return new JsonResult(messageFail);
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

        //Get RequestMedia Filter Status
        [HttpPost]
        [Route("GetRequestMediaFilterStatus")]
        public IActionResult GetRequestMediaFilterStatus([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<MediaEditRequest> requests = editorService.GetRequestMediaFilterStatus(request.UserEmail, request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfRequestMediaFilterStatus(request.UserEmail, request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = requests
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetRequestMediaFilterStatus: " + e.Message);
                return new JsonResult(new
                {
                    Message = "fail"
                });
            }
        }

        //Search RequestMedia Filter Status
        [HttpPost]
        [Route("SearchingRequestMediaFilterStatus")]
        public IActionResult SearchingRequestMediaFilterStatus([FromServices] IEditorService editorService,
            [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<MediaEditRequest> requests = editorService.SearchingRequestMediaFilterStatus(request.SearchValue, request.UserEmail, request.Status, request.RowsOnPage, request.RequestPage);
                int count = editorService.NumberOfSearchingRequestMediaFilterStatus(request.SearchValue, request.UserEmail, request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = requests
                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("SearchingRequestMediaFilterStatus: " + e.Message);
                return new JsonResult(new
                {
                    Message = "fail"
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
                string mediaID = editorService.AddMedia(request.Title, request.FilmType, request.Director,
                    request.Cast, request.PublishYear, request.Duration, request.BannerURL, request.Language, request.Description);
                if (mediaID == null) return new JsonResult(messageFail);
                if (request.Note.Trim() == "") request.Note = "Request add media";
                bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, mediaID, request.Note, "media", mediaID);
                if (!addMediaRequest) return new JsonResult(messageFail);
                for (int i = 0; i < request.CategoryIDArray.Length; i++)
                {
                    bool addMediaCategory = editorService.AddMediaCategory(mediaID, request.CategoryIDArray[i]);
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

        public partial class JsonCategory
        {
            public string CategoryID { get; set; }
            public string Name { get; set; }
        }

        [Route("AddCategory")]
        [HttpPost]
        public IActionResult AddCategory([FromServices] IEditorService editorService,
                                        [FromBody] JsonCategory _jCategory)
        {
            string categoryIdErr = "";
            string message = "Fail";
            string nameErr = "";
            try
            {
                bool isValid = true;
                uint categoryID;
                bool isValidCategoryId = uint.TryParse(_jCategory.CategoryID, out categoryID);
                if (isValidCategoryId == true)
                {
                    Category _category = editorService.GetCategoryById((int)categoryID);
                    if (_category != null)
                    {
                        categoryIdErr = "CategoryID is existed";
                        isValid = false;
                    }
                }
                else
                {
                    categoryIdErr = "CategoryID is Invalid";
                    isValid = false;
                }
                if (_jCategory.Name.Equals(""))
                {
                    nameErr = "Name must not be empty";
                    isValid = false;
                }
                if (isValid == true)
                {
                    var category = new Category
                    {
                        CategoryID = (int)categoryID,
                        Name = _jCategory.Name,
                    };
                    bool result = editorService.AddCategory(category);
                    if (result == true) message = "Success";
                }
                else message = "Fail";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CreateStaff: " + ex.Message);
                message = "Fail" + ex.Message;
            }
            return new JsonResult(new { Message = message, CategoryErr = categoryIdErr, NameErr = nameErr });
        }

        public class JSeasonForm
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
        public IActionResult AddSeason([FromServices] IEditorService editorService, [FromBody] JSeasonForm frmSeason)
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
                bool addMediaRequest = editorService.AddMediaRequest(frmSeason.UserEmail, frmSeason.MediaID, "add new season", "season", seasonID);
                if(addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
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
        public IActionResult EditSeason([FromServices] IEditorService editorService, [FromBody] JSeasonForm frmSeason)
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
                bool addMediaRequest = editorService.AddMediaRequest(frmSeason.UserEmail, frmSeason.MediaID, "update the season", "season", seasonID);
                if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
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

        public class JEpisodeForm
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
        public IActionResult AddEpisode([FromServices] IEditorService editorService, [FromBody] JEpisodeForm frmEpisode)
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
                bool addMediaRequest = editorService.AddMediaRequest(frmEpisode.UserEmail, media.MediaID, "add new episode", type, episodeID);
                if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
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
        public IActionResult EditEpisode([FromServices] IEditorService editorService, [FromBody] JEpisodeForm frmEpisode)
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
                bool addMediaRequest = editorService.AddMediaRequest(frmEpisode.UserEmail, media.MediaID, "update episode", type, episodeID);
                if (addMediaRequest == false) return new JsonResult(new { Message = "Fail" });
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
    }
}
