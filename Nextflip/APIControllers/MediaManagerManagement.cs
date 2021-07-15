using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.mediaEditRequest;
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

        public MediaManagerManagement(ILogger<MediaManagerManagement> logger)
        {
            _logger = logger;
        }

        /*[Route("GetAllPendingMedias")]
        public JsonResult GetAllPendingMedias([FromServices] IMediaManagerManagementService mediaManagerManagementService)
        {
            try
            {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetAllPendingMedias();
            return new JsonResult(requests);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllPendingMedias: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        //Search all
        [Route("GetPendingMediaByUserEmail")]
        public JsonResult GetPendingMediaByUserEmail([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediaByUserEmail(request.SearchValue.Trim(), request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfPendingMediasBySearching(request.SearchValue.Trim());
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
                _logger.LogInformation("GetPendingMediaByUserEmail: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        //Search all + filter status
        [Route("GetPendingMediaByUserEmailFilterStatus")]
        public JsonResult GetPendingMediaByUserEmailFilterStatus([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediaByUserEmailFilterStatus(request.SearchValue.Trim(), request.Status, request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfPendingMediasBySearchingFilterStatus(request.SearchValue.Trim(), request.Status);
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
                _logger.LogInformation("GetPendingMediaByUserEmailFilterStatus: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }
        */

        [Route("ApproveRequest")]
        public JsonResult ApproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                bool approveRequest = mediaManagerManagementService.ApproveRequest(request.RequestID);
                if (!approveRequest) return new JsonResult(messageFail);
                MediaEditRequest editRequest = mediaManagerManagementService.GetMediaEditRequestByID(request.RequestID);
                bool approveChange = false;
                if (editRequest.type.Trim().Equals("media"))
                    approveChange = mediaManagerManagementService.ApproveChangeMedia(editRequest.ID);
                else if (editRequest.type.Trim().Equals("season"))
                    approveChange = mediaManagerManagementService.ApproveChangeSeason(editRequest.ID);
                else if (editRequest.type.Trim().Equals("episode"))
                    approveChange = mediaManagerManagementService.ApproveChangeEpisode(editRequest.ID);
                else if (editRequest.type.Trim().Equals("subtitle"))
                    approveChange = mediaManagerManagementService.ApproveChangeSubtitle(editRequest.ID);
                if (!approveChange) return new JsonResult(messageFail);
                var message = new
                {
                    message = "success"
                };
                return new JsonResult(message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ApproveRequest: " + ex.Message);
                var result = new
                {
                    message = ex.Message
                };
                return new JsonResult(result);
            }

        }

        [Route("DisapproveRequest")]
        public async Task<IActionResult> DisapproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService,
                                        [FromServices] ISendMailService sendMailService, [FromBody] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                bool disapproveRequest = mediaManagerManagementService.DisappoveRequest(request.RequestID, request.note);
                if (!disapproveRequest) return new JsonResult(messageFail);
                MediaEditRequest editRequest = mediaManagerManagementService.GetMediaEditRequestByID(request.RequestID);
                bool disapproveChange = false;
                if (editRequest.type.Trim().Equals("media"))
                    disapproveChange = mediaManagerManagementService.DisapproveChangeMedia(editRequest.ID);
                else if (editRequest.type.Trim().Equals("season"))
                    disapproveChange = mediaManagerManagementService.DisapproveChangeSeason(editRequest.ID);
                else if (editRequest.type.Trim().Equals("episode"))
                    disapproveChange = mediaManagerManagementService.DisapproveChangeEpisode(editRequest.ID);
                else if (editRequest.type.Trim().Equals("subtitle"))
                    disapproveChange = mediaManagerManagementService.DisapproveChangeSubtitle(editRequest.ID);
                if (!disapproveChange) return new JsonResult(messageFail);
                string toEmail = editRequest.userEmail;
                string body = $"Dear,\n" +
                                $"your request about: << {editRequest.mediaTitle} >> is disapproved \n" +
                                $"Because: {request.note}";
                await sendMailService.SendEmailAsync(toEmail, "Notification of your request", body);
                var message = new
                {
                    message = "success"
                };
                return new JsonResult(message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ApproveRequest: " + ex.Message);
                var result = new
                {
                    message = ex.Message
                };
                return new JsonResult(result);
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
                if (request.SortBy.Trim() == "") request.SortBy = "asc";
                if (request.Type.Trim() == "") request.Type = "all";
                if (request.Status.Trim() == "") request.Status = "all";
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetMediaRequest(request.Status.Trim().ToLower(), 
                    request.Type.Trim().ToLower(), request.SortBy.Trim().ToLower(), request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfMediaRequest(request.Status.Trim().ToLower(), request.Type.Trim().ToLower());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = requests
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
                if (request.SortBy.Trim() == "") request.SortBy = "asc";
                if (request.Type.Trim() == "") request.Type = "all";
                if (request.Status.Trim() == "") request.Status = "all";
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.SearchingMediaRequest(request.SearchValue.Trim(), 
                    request.Status.Trim().ToLower(), request.SortBy, request.Type.Trim().ToLower(), request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfMediaRequestSearching(request.SearchValue.Trim(), request.Status.Trim().ToLower(), request.Type.Trim().ToLower());
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = requests
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

    }
}
