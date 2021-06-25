using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.category;
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
            public string SearchValue { get; set; }
            public string MediaID { get; set; }
            public string UserEmail { get; set; }
            public string note { get; set; }
            public int CategoryID { get; set; }
            public string Status { get; set; }
            public string CategoryName { get; set; }
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }

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
        [Route("RequestDisabledMedia")]
        public IActionResult RequestDisabledMedia([FromServices] IMediaService mediaService,
            [FromServices] IMediaManagerManagementService mediaManagerManagementService, [FromBody] Request request)
        {
            try
            {
                bool changeMediaStatus = mediaService.ChangeMediaStatus(request.MediaID, "Pending");
                bool addMediaRequest = mediaManagerManagementService.AddMediaRequest(request.UserEmail, request.MediaID, request.note);
                var result = new
                {

                };
                return (new JsonResult(result));
            }
            catch (Exception e)
            {
                _logger.LogInformation("RequestDisabledMedia: " + e.Message);
                return new JsonResult("An error occurred");
            }
        }

    }
}
