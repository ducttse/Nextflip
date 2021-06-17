using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.media;
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
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        [Route("GetMediasByTitle/{searchValue}")]
        public JsonResult GetMediasByTitle([FromServices] IMediaService mediaService, [FromBody] Request request, string searchValue)
        {
            try
            {
                IEnumerable<Media> medias = mediaService.GetMediasByTitle(searchValue, request.RowsOnPage, request.RequestPage);
                int count = mediaService.NumberOfMediasBySearching(searchValue);
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
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
        [Route("GetMedias")]
        public IActionResult GetMedias([FromServices] IMediaService mediaService,
            [FromBody] Request request)
        {
            try
            {
                IEnumerable<Media> medias = mediaService.GetMedias(request.RowsOnPage, request.RequestPage);
                int count = mediaService.NumberOfMedias();
                double totalPage = (double)count / (double)request.RowsOnPage;
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
    }
}
