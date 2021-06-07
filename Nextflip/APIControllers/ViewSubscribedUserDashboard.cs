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
        public JsonResult GetCategories([FromServices] ICategoryService categoryService)
        {
            IEnumerable<CategoryDTO> categories = categoryService.GetCategories();
            return new JsonResult(categories) ;
        }

        [Route("GetMediasByTitle/{searchValue}")]
        public JsonResult GetMediasByTitle([FromServices] IMediaService mediaService, string searchValue)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetMediasByTitle(searchValue);
            return new JsonResult(medias);
        }

        [Route("GetFavoriteMedias/{userID}")]
        public JsonResult GetFavoriteMedias([FromServices] IMediaService mediaService, string userID)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetFavoriteMediasByUserID(userID);
            return new JsonResult(medias);
        }

        [Route("GetMediasByCategoryID/{categoryID}")]
        public JsonResult GetMediasByCategoryID([FromServices] IMediaService mediaService, int categoryID)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetMediasByCategoryID(categoryID);
            return new JsonResult(medias);
        }
    }
}
