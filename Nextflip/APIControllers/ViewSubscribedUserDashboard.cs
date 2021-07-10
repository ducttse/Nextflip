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
        public IActionResult GetCategories([FromServices] ISubscribedUserService subscribedUserService)
        {
            try
            {
                IEnumerable<Category> categories = subscribedUserService.GetCategories();
                return new JsonResult(categories);
            }catch(Exception ex)
            {
                _logger.LogInformation("GetCategories: " + ex.Message);
                return new JsonResult("error occur");
            }
        }

        [Route("GetMediasByTitle/{searchValue}")]
        public IActionResult GetMediasByTitle([FromServices] ISubscribedUserService subscribedUserService, string searchValue)
        {
            
            try
            {
                IEnumerable<Media> medias = subscribedUserService.GetMediasByTitle(searchValue);
                var mediaListWithCategory = new List<dynamic>();
                if (medias != null)
                {
                    foreach(var media in medias)
                    {
                        IEnumerable<Category> categories = subscribedUserService.GetCategoriesByMediaID(media.MediaID);
                        mediaListWithCategory.Add(new { 
                                                       Media = media,
                                                       Categories = categories                                                    
                                                    });
                    }
                }
                mediaListWithCategory.Reverse();
                return new JsonResult(mediaListWithCategory);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByTitle: " + ex.Message);
                return new JsonResult("error occur: ");
            }
        }


        [Route("GetMediasByCategoryID/{categoryID}/{limit=10}")]
        public IActionResult GetMediasByCategoryID([FromServices] ISubscribedUserService subscribedUserService, int categoryID, int limit)
        {
            try
            {
                IEnumerable<Media> medias = subscribedUserService.GetMediasByCategoryID(categoryID,limit);
                return new JsonResult(medias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByCategoryID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
        public partial class NewestMediaCategory
        {
            public IEnumerable<Media> Newest { get; set; }
            public IEnumerable<Media> Action { get; set; }
            public IEnumerable<Media> Adventure { get; set; }
            public IEnumerable<Media> Comedy { get; set; }
            public IEnumerable<Media> Romance { get; set; }
            public IEnumerable<Media> Horror { get; set; }

        }
        [Route("GetMedias/{limit=10}")]
        public IActionResult GetMedias([FromServices] ISubscribedUserService subscribedUserService, int limit)
        {
            try
            {
                NewestMediaCategory array = new NewestMediaCategory
                {
                    Newest = subscribedUserService.GetNewestMedias(limit),
                    Action = subscribedUserService.GetMediasByCategoryID(1,limit),
                    Adventure = subscribedUserService.GetMediasByCategoryID(2, limit),
                    Comedy = subscribedUserService.GetMediasByCategoryID(3, limit),
                    Romance = subscribedUserService.GetMediasByCategoryID(4, limit),
                    Horror = subscribedUserService.GetMediasByCategoryID(5, limit)
                };
                return new JsonResult(array);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMedias: " + ex.Message);
                return new JsonResult("error occur");
            }
        }

        public partial class Favorite
        {
            public string UserID { get; set; }
            public int Limit { get; set; } = 8;
            public int Page { get; set; } = 1;
        }
        [Route("GetFavoriteList")]
        [HttpPost]
        public IActionResult GetFavoriteList([FromServices] ISubscribedUserService subscribedUserService, Favorite favorite)
        {
            try
            {
                IEnumerable<Media> favoriteList = subscribedUserService.GetFavoriteMediasByUserID(favorite.UserID, favorite.Limit,favorite.Page);
                return new JsonResult(favoriteList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetFavoriteList: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
    }
}
