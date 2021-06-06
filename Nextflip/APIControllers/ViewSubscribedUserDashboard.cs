using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.category;
using Nextflip.Models.media;

namespace Nextflip.APIControllers
{
    public class ViewSubscribedUserDashboard : ControllerBase
    {

        public JsonResult GetCategories(ICategoryService categoryService)
        {
            IEnumerable<CategoryDTO> categories = categoryService.GetCategories();
            return new JsonResult(categories) ;
        }

        public JsonResult GetMediasByTitle(IMediaService mediaService,[FromForm] string searchValue)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetMediasByTitle(searchValue);
            return new JsonResult(medias);
        }

        public JsonResult GetFavoriteMedias(IMediaService mediaService,[FromForm] string userID)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetFavoriteMediasByUserID(userID);
            return new JsonResult(medias);
        }

        public JsonResult GetMediasByCategoryID(IMediaService mediaService, [FromForm] int categoryID)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetMediasByCategoryID(categoryID);
            return new JsonResult(medias);
        }
    }
}
