using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.category;
using Nextflip.Models.media;
using Microsoft.Extensions.Logging;
using System;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewSubscribedUserDashboard : ControllerBase
    {
        private readonly ILogger _logger;

        public ViewSubscribedUserDashboard(ILogger<ViewSubscribedUserDashboard> logger)
        {
            _logger = logger;
        }

        [Route("GetCategories")]
        public IActionResult GetCategories([FromServices] ICategoryService categoryService)
        {
            try
            {
                IEnumerable<Category> categories = categoryService.GetCategories();
                return new JsonResult(categories);
            }catch(Exception ex)
            {
                _logger.LogInformation("GetCategories: " + ex.Message);
                return null;
            }
        }

        [Route("GetMediasByTitle/{searchValue}")]
        public IActionResult GetMediasByTitle([FromServices] IMediaService mediaService, string searchValue)
        {
            try
            {
                IEnumerable<Media> medias = mediaService.GetMediasByTitle(searchValue);
                return new JsonResult(medias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return null;
            }
        }

        [Route("GetFavoriteMedias/{userID}")]
        public IActionResult GetFavoriteMedias([FromServices] IMediaService mediaService, string userID)
        {
            try
            {
                IEnumerable<Media> medias = mediaService.GetFavoriteMediasByUserID(userID);
                return new JsonResult(medias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetFavoriteMedias: " + ex.Message);
                return null;
            }
        }

        [Route("GetMediasByCategoryID/{categoryID}")]
        public IActionResult GetMediasByCategoryID([FromServices] IMediaService mediaService, int categoryID)
        {
            try
            {
                IEnumerable<Media> medias = mediaService.GetMediasByCategoryID(categoryID);
                return new JsonResult(medias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByCategoryID: " + ex.Message);
                return null;
            }
        }
    }
}
