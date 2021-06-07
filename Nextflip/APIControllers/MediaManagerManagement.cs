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
    public class MediaManagerManagement : ControllerBase
    {
        [Route("api/MediaManagerManagement/")]
        public JsonResult GetAllPendingMedias([FromServices] IMediaManagerManagementService mediaManagerManagementService)
        {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetAllPendingMedias();
            Debug.WriteLine("FLAG");
            return new JsonResult(requests);
        }

        public JsonResult GetPendingMediaByUserEmail([FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] string searchValue)
        {
            IEnumerable<MediaEditRequest> requests = mediaManagerManagementService.GetPendingMediaByUserEmail(searchValue);
            Debug.WriteLine("FLAG");
            return new JsonResult(requests);
        }

        
    }
}
