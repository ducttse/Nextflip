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


    }
}
