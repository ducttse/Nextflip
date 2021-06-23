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
                return new JsonResult("error occur");
            }
        }

        [Route("GetFavoriteMedias/{userID}")]
        public IActionResult GetFavoriteMedias([FromServices] ISubscribedUserService subscribedUserService, string userID)
        {
            try
            {
                IEnumerable<Media> medias = subscribedUserService.GetFavoriteMediasByUserID(userID);
                var mediaListWithCategory = new List<dynamic>();
                if (medias != null)
                {
                    foreach (var media in medias)
                    {
                        IEnumerable<Category> categories = subscribedUserService.GetCategoriesByMediaID(media.MediaID);
                        mediaListWithCategory.Add(new
                        {
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
                _logger.LogInformation("GetFavoriteMedias: " + ex.Message);
                return new JsonResult("error occur");
            }
        }

        [Route("GetMediasByCategoryID/{categoryID}")]
        public IActionResult GetMediasByCategoryID([FromServices] ISubscribedUserService subscribedUserService, int categoryID)
        {
            try
            {
                IEnumerable<Media> medias = subscribedUserService.GetMediasByCategoryID(categoryID);
                return new JsonResult(medias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetMediasByCategoryID: " + ex.Message);
                return new JsonResult("error occur");
            }
        }
    }
}
