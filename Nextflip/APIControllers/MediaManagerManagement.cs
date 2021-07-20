using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaManagerManagement : ControllerBase
    {
        private readonly ILogger _logger;
        private const string DEFAULT_ACCOUNT = "technical.nextflipcompany@gmail.com";
        public MediaManagerManagement(ILogger<MediaManagerManagement> logger)
        {
            _logger = logger;
        }
        public partial class MediaForm
        {
            public string RequestID { get; set; }
            public string Type { get; set; }

            public string EditorEmail { get; set; }
            public string Content { get; set; }
        }

        

        [Route("ApproveRequest")]
        public JsonResult ApproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] MediaForm request)
        {
            try
            {
                bool isValid = true;
                switch (request.Type.ToLower())
                {
                    case "media":
                        isValid = mediaManagerManagementService.ApproveChangeMedia(request.RequestID);
                        break;
                    case "season":
                        isValid = mediaManagerManagementService.ApproveChangeSeason(request.RequestID);
                        break;
                    case "episode":
                        isValid = mediaManagerManagementService.ApproveChangeEpisode(request.RequestID);
                        break;
                    default:
                        isValid = false;
                        break;
                }
                if (!isValid) return new JsonResult(new { Message = "Failed" });
                return new JsonResult(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ApproveRequest: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }

        }

        [Route("DisapproveRequest")]
        public async Task<JsonResult> DisapproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromServices] ISendMailService sendMailService, [FromBody] MediaForm request)
        {
            try
            {
                bool isValid = true;
                switch (request.Type.ToLower())
                {
                    case "media":
                        isValid = mediaManagerManagementService.DisapproveChangeMedia(request.RequestID);
                        break;
                    case "season":
                        isValid = mediaManagerManagementService.DisapproveChangeSeason(request.RequestID);
                        break;
                    case "episode":
                        isValid = mediaManagerManagementService.DisapproveChangeEpisode(request.RequestID);
                        break;
                    default:
                        isValid = false;
                        break;
                }
                if (!isValid) return new JsonResult(new { Message = "Failed" });
                string toEmail = DEFAULT_ACCOUNT;
                string body = $"Dear Editor,\n" +
                                $"Your request about: << " + request.Type + " - " + request.RequestID + " >> is disapproved \n" +
                                $"Because: {request.Content}";
                await sendMailService.SendEmailAsync(toEmail, "Disapproval of your request", body);
                return new JsonResult(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ApproveRequest: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        /*
        [Route("NumberOfPendingMedias")]
        public JsonResult NumberOfPendingMedias([FromServices] IMediaManagerManagementService mediaManagerManagementService)
        {
            try
            {
                int count = mediaManagerManagementService.NumberOfPendingMedias();
                return new JsonResult(count);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("NumberOfPendingMedias: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }
        */
        public class Request
        {
            public int RequestID { get; set; }
            public string MediaID { get; set; }
            public string EpisodeID { get; set; }
            public string SortBy { get; set; }
            public string note { get; set; }
            public string SearchValue { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public string CategoryName { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        /*/Get All
        [HttpPost]
        [Route("GetPendingMediasListAccordingRequest")]
        public JsonResult GetPendingMediasListAccordingRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService,
                                [FromBody] Request request)
        {
            try
            {
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediasListAccordingRequest(request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfPendingMedias();
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = requests
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetPendingMediasListAccordingRequest: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }
        
        //Get All + Filter Status
        [HttpPost]
        [Route("GetPendingMediasFilterStatus")]
        public JsonResult GetPendingMediasFilterStatus([FromServices] IMediaManagerManagementService mediaManagerManagementService,
                                [FromBody] Request request)
        {
            try
            {
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediasFilterStatus(request.Status, request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfPendingMediasFilterStatus(request.Status);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = requests
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetPendingMediasFilterStatus: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }
        */
        //Get
        [HttpPost]
        [Route("GetMediaRequest")]
        public JsonResult GetMediaRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService,
                                [FromBody] Request request)
        {
            try
            {
                if (request.Status.Trim() == "") request.Status = "all";
                if (request.CategoryName.Trim() == "") request.CategoryName = "all";
                IEnumerable<Media> mediaList = mediaManagerManagementService.ViewMediasFilterCategory_Status(request.CategoryName.Trim().ToLower(),
                    request.Status.Trim(), request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfMediasFilterCategory_Status(request.CategoryName.Trim().ToLower(), request.Status.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = mediaList
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
        [Route("SearchingMediaRequest")]
        public JsonResult SearchingMediaRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
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
                IEnumerable<Media> mediaList = mediaManagerManagementService.GetMediasByTitleFilterCategory_Status(request.SearchValue.Trim().ToLower(),
                    request.CategoryName.Trim().ToLower(), request.Status.Trim(), request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfMediasBySearchingFilterCategory_Status(request.SearchValue.Trim().ToLower(),
                    request.CategoryName.Trim().ToLower(), request.Status.Trim());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = mediaList
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

        //Get Media By ID
        [Route("GetMediaByID")]
        public JsonResult GetMediaByID([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                Media media = mediaManagerManagementService.GetMediaByID(request.MediaID);
                var result = new
                {
                    Message = "success",
                    Data = media
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaByID: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("GetEpisodeByID")]
        public JsonResult GetEpisodeByID([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                Episode episode = mediaManagerManagementService.GetEpisodeByID(request.EpisodeID);
                var result = new
                {
                    Message = "success",
                    Data = episode
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetEpisodeByID: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("GetMediaEditRequestByID/{RequestID}")]
        public JsonResult GetMediaEditRequestByID([FromServices] IMediaManagerManagementService mediaManagerManagementService, int RequestID)
        {
            try
            {
                int a;
                if (!int.TryParse(RequestID.ToString(), out a) || RequestID <= 0) return new JsonResult(new { Message = "fail" });
                MediaEditRequest EditRequest = mediaManagerManagementService.GetMediaEditRequestByID(RequestID);
                var result = new
                {
                    Message = "success",
                    Data = EditRequest
                };
                if (EditRequest != null) return (new JsonResult(result));
                else return new JsonResult(new { Message = "fail" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediaEditRequestByID: " + ex.Message);
                return new JsonResult(new
                {
                    message = "fail"
                });
            }
        }

        [Route("GetDetailedMedia/{mediaID}")]
        [HttpGet]
        public JsonResult GetDetailedMedia([FromServices] IMediaManagerManagementService mediaManagerManagementService, string mediaID)
        {
            try
            {
                ViewEditorDashboard.PrototypeMediaForm detailedMedia =
                    mediaManagerManagementService.GetDetailedMediaByMediaId(mediaID);
                return new JsonResult(detailedMedia);
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Get Detailed Media by media ID: " + exception.Message);
                return new JsonResult(new {message = "operation failed"});
            }
        }

    }
}
