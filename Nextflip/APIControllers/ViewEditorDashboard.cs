using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models;
using Nextflip.Models.category;
using Nextflip.Models.media;
using Nextflip.Models.mediaEditRequest;
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
            public string LinkPreview { get; set; }
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
                bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, request.MediaID, request.Note, request.LinkPreview, "", request.MediaID);
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
                addMediaRequest = editorService.AddMediaRequest(request.UserEmail, mediaByChildID.MediaID, request.Note, request.LinkPreview, request.Type, request.ID);

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
                IEnumerable<MediaEditRequest> requests = editorService.SearchingRequestMediaFilterStatus(request.SearchValue ,request.UserEmail, request.Status, request.RowsOnPage, request.RequestPage);
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
        public IActionResult AddMedia([FromServices] IEditorService editorService, [FromForm] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                string mediaID = editorService.AddMedia(request.Title, request.FilmType, request.Director,
                    request.Cast, request.PublishYear, request.Duration, request.BannerURL, request.Language, request.Description);
                bool addMediaRequest = editorService.AddMediaRequest(request.UserEmail, mediaID, request.Note, request.LinkPreview, request.Type, mediaID);
                if (mediaID == null || !addMediaRequest) return new JsonResult(messageFail);
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
    }
}
