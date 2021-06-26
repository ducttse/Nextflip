﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [Route("GetAllPendingMedias")]
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
                return new JsonResult("Error occur");
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
                return new JsonResult("Error occur");
            }
        }

        [Route("ApproveRequest")]
        public JsonResult ApproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromForm] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                bool approveChangeMediaStatusRequest = mediaManagerManagementService.ApproveRequest(request.RequestID);
                bool approveChangeMediaStatus = mediaManagerManagementService.ApproveChangeMediaStatus(request.MediaID);
                if (!approveChangeMediaStatusRequest || !approveChangeMediaStatus) return new JsonResult(messageFail);
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
        public JsonResult DisapproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromForm] Request request)
        {
            try
            {
                var messageFail = new
                {
                    message = "fail"
                };
                bool disapproveRequest = mediaManagerManagementService.DisappoveRequest(request.RequestID, request.note);
                if (!disapproveRequest) return new JsonResult(messageFail);
                bool disapproveChangeMediaStatus = mediaManagerManagementService.DisapproveChangeMediaStatus(request.MediaID);
                if (!disapproveChangeMediaStatus) return new JsonResult(messageFail);
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
                return new JsonResult("Error occur");
            }
        }

        public class Request
        {
            public int RequestID { get; set; }
            public string MediaID { get; set; }
            public string note { get; set; }
            public string SearchValue { get; set; }
            public string Status { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        //Get All
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
                return new JsonResult("Error occur");
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
                return new JsonResult("Error occur");
            }
        }

    }
}
