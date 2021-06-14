using Microsoft.AspNetCore.Mvc;
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
        [Route("GetAllPendingMedias")]
        public JsonResult GetAllPendingMedias([FromServices] IMediaManagerManagementService mediaManagerManagementService)
        {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetAllPendingMedias();
            Debug.WriteLine("FLAG");
            return new JsonResult(requests);
        }

        [Route("GetPendingMediaByUserEmail/{searchValue}")]
        public JsonResult GetPendingMediaByUserEmail([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] string searchValue)
        {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediaByUserEmail(searchValue);
            Debug.WriteLine("FLAG");
            return new JsonResult(requests);
        }

        [Route("ApproveRequest/{requestID}")]
        public JsonResult ApproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] int requestID)
        {
            bool result = mediaManagerManagementService.ApproveRequest(requestID);
            Debug.WriteLine("FLAG");
            return new JsonResult(result);
        }

        [Route("DisapproveRequest/{requestID}")]
        public JsonResult DisapproveRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] int requestID)
        {
            bool result = mediaManagerManagementService.DisappoveRequest(requestID);
            Debug.WriteLine("FLAG");
            return new JsonResult(result);
        }

        [Route("NumberOfPendingMedias")]
        public JsonResult NumberOfPendingMedias([FromServices] IMediaManagerManagementService mediaManagerManagementService)
        {
            int count = mediaManagerManagementService.NumberOfPendingMedias();
            return new JsonResult(count);
        }
        public class Request
        {
            public int NumberOfPage { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        [HttpPost]
        [Route("GetPendingMediasListAccordingRequest")]
        public JsonResult GetPendingMediasListAccordingRequest([FromServices] IMediaManagerManagementService mediaManagerManagementService,
                                [FromBody] Request request)
        {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediasListAccordingRequest(request.NumberOfPage, request.RowsOnPage, request.RequestPage);
            return new JsonResult(requests);
        }
    }
}
