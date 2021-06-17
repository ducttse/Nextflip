using Microsoft.AspNetCore.Mvc;
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

        [Route("GetPendingMediaByUserEmail/{searchValue}")]
        public JsonResult GetPendingMediaByUserEmail([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request, string searchValue)
        {
            try
            {
                IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediaByUserEmail(searchValue, request.RowsOnPage, request.RequestPage);
                int count = mediaManagerManagementService.NumberOfPendingMediasBySearching(searchValue);
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

        [Route("ApproveRequest/{requestID}")]
        public JsonResult ApproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] int requestID)
        {
            try
            {
                bool result = mediaManagerManagementService.ApproveRequest(requestID);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ApproveRequest: " + ex.Message);
                return new JsonResult("Error occur");
            }

        }

        [Route("DisapproveRequest/{requestID}")]
        public JsonResult DisapproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] int requestID)
        {
            try
            {
                bool result = mediaManagerManagementService.DisappoveRequest(requestID);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("DisapproveRequest: " + ex.Message);
                return new JsonResult("Error occur");
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
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

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
    }
}
