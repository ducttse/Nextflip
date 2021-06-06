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
    public class ViewMediaDetails : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetSeasons()
        {
            string mediaID = "";
            ISeasonService seasonService = new SeasonService();
            List<Season> seasons = (List<Season>)seasonService.GetSeasonsByMediaID(mediaID);
            return new JsonResult(seasons);
        }

        public JsonResult GetEpisodes()
        {
            string seasionID = "";
            IEpisodeService episodeService = new EpisodeService();
            List<Episode> episodes = (List<Episode>)episodeService.GetEpisodesBySeasonID(seasionID);
            return new JsonResult(episodes);
        }

        
    }
}
