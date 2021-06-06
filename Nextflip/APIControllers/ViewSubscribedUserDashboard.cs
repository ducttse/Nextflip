using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Nextflip.Services.Interfaces;
using Nextflip.Models.category;
using Nextflip.Models.media;

namespace Nextflip.Controllers
{
    public class ViewSubscribedUserDashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetCategories(ICategoryService categoryService)
        {
            IEnumerable<CategoryDTO> categories = categoryService.GetCategories();
            return new JsonResult(categories) ;
        }

        public JsonResult GetMediasByTitle(IMediaService mediaService,[FromForm] string searchValue)
        {
            IEnumerable<MediaDTO> medias = (List<MediaDTO>)mediaService.GetMediasByTitle(searchValue);
            return new JsonResult(medias);
        }

        public JsonResult GetFavoriteMedias(IMediaService mediaService,[FromForm] string userID)
        {
            IEnumerable<MediaDTO> medias = mediaService.GetFavoriteMediasByUserID(userID);
            return new JsonResult(medias);
        }
    }
}
