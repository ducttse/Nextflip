using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Services.Implementations;
using Nextflip.Services.Interfaces;
using Nextflip.Models.DTO;
using System.Text.Json;

namespace Nextflip.Controllers
{
    public class ViewSubscribedUserDashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetCategories()
        {
            ICategoryService categoryService = new CategoryService();
            List<Category> categories = (List<Category>)categoryService.GetCategories();
            return new JsonResult(categories) ;
        }

        public JsonResult GetMediasByTitle()
        {
            string searchValue = "";
            IMediaService mediaService = new MediaService();
            List<Media> medias = (List<Media>)mediaService.GetMediasByTitle(searchValue);
            return new JsonResult(medias);
        }

        public JsonResult GetFavoriteMedias()
        {
            string userID = "";
            IMediaService mediaService = new MediaService();
            List<Media> medias = (List<Media>)mediaService.GetFavoriteMediasByUserID(userID);
            return new JsonResult(medias);
        }
    }
}
