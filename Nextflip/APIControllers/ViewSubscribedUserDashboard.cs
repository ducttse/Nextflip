using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.category;
using Nextflip.Models.media;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewSubscribedUserDashboard : ControllerBase
    {
        
        [Route("GetCategories")]
        public IActionResult GetCategories([FromServices] ICategoryService categoryService)
        {
            IEnumerable<Category> categories = categoryService.GetCategories();
            return new JsonResult(categories) ;
        }

        [Route("GetMediasByTitle/{searchValue}")]
        public IActionResult GetMediasByTitle([FromServices] IMediaService mediaService, string searchValue)
        {
            IEnumerable<Media> medias = mediaService.GetMediasByTitle(searchValue);
            return new JsonResult(medias);
        }

        [Route("GetFavoriteMedias/{userID}")]
        public IActionResult GetFavoriteMedias([FromServices] IMediaService mediaService, string userID)
        {
            IEnumerable<Media> medias = mediaService.GetFavoriteMediasByUserID(userID);
            return new JsonResult(medias);
        }

        [Route("GetMediasByCategoryID/{categoryID}")]
        public IActionResult GetMediasByCategoryID([FromServices] IMediaService mediaService, int categoryID)
        {
            IEnumerable<Media> medias = mediaService.GetMediasByCategoryID(categoryID);
            return new JsonResult(medias);
        }
    }
}
