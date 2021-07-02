using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddMedia : ControllerBase
    {
        private readonly ILogger _logger;
        
        public AddMedia(ILogger<AddMedia> logger)
        {
            _logger = logger;
        }

        [Route("AddPreviewMedia")]
        [HttpPost]
        public IActionResult AddPreviewMedia([FromServices] IMediaService mediaService,
            [FromBody] AddMediaModel addMediaModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = mediaService.AddPreviewMedia(addMediaModel);
                    return new JsonResult(result);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }

            }
            return NotFound();
        }


        public class AddMediaModel
        {
            public string Status { get; set; }
            public string Title { get; set; }
            public string FilmType { get; set; }
            public string Director { get; set; }
            public string Cast { get; set; }
            public int? PublishYear { get; set; }
            public string Duration { get; set; }
            public string Description { get; set; }

            public int NumberOfSeason { get; set; }
            public string Language { get; set; }
            public IList<int> NumberOfEpisodeList { get; set; }

        }
    }
}
