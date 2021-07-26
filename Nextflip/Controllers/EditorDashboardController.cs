using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nextflip.Services.Interfaces;

namespace Nextflip.Controllers
{
    [Authorize(Policy = "media editor")]

    public class EditorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string id, [FromServices] IMediaService mediaService)
        {
            var mediaInfo = mediaService.GetMediaByID(id);
            if (mediaInfo.Status == "Approved")
            {
                TempData["clone message"] =
                    "You are trying to edit a published media so we create a drafted version of the media. " +
                    "Change to the published media will be reflect once Media Manager approve to the new version. " +
                    "Feel free to delete the draft version if needed.";
                TempData["oldMediaID"] = id;
                var newMediaID = mediaService.CloneMedia(id);
                RedirectToAction("Edit", "EditorDashboard", newMediaID);
            }
            return View();
        }


        public IActionResult ViewEditRequest()
        {
            return View();
        }
        public IActionResult ViewAddNewMedia()
        {
            return View();
        }
        [HttpGet("/EditorDashboard/ViewEditMedia/{id}")]
        public IActionResult ViewEditMedia(String id, [FromServices] IMediaService mediaService)
        {
            ViewBag.MediaID = id;
            var mediaInfo = mediaService.GetMediaByID(id);
            if (mediaInfo.Status == "Approved")
            {
                TempData["clone message"] =
                    "You are trying to edit a published media so we create a drafted version of the media. " +
                    "Change to the published media will be reflect once Media Manager approve to the new version. " +
                    "Feel free to delete the draft version if needed.";
                TempData["oldMediaID"] = id;
                var newMediaID = mediaService.CloneMedia(id);
                RedirectToAction("ViewEditMedia", "EditorDashboard", newMediaID);
            }
            return View();
        }
    }
}
