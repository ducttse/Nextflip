using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Models.filmType;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmTypeManagement : ControllerBase
    {
        private readonly ILogger _logger;
        public FilmTypeManagement(ILogger<FilmTypeManagement> logger)
        {
            _logger = logger;
        }

        [Route("UpdateFilmType/{typeID}/{type}")]
        public IActionResult UpdateFilmType([FromServices] IFilmTypeService filmTypeService, int typeID, string type)
        {
            try
            {
                bool result = filmTypeService.UpdateFilmType(typeID, type);
                if (result) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("FilmTypeManagement/UpdateFilmType: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("CreateNewFilmType/{filmTypeName}")]
        public IActionResult CreateNewFilmType([FromServices] IFilmTypeService filmTypeService, string filmTypeName)
        {
            try
            {
                bool result = filmTypeService.CreateNewFilmType(filmTypeName);
                if (result) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("FilmTypeManagement/CreateNewFilmType: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("GetAllFilmTypes")]
        public IActionResult GetAllFilmTypes([FromServices] IFilmTypeService filmTypeService)
        {
            try
            {
                IEnumerable<FilmType> filmTypes = filmTypeService.GetFilmTypes();
                if (filmTypes != null) return new JsonResult(filmTypes);
                return new JsonResult(new { Message = "An error occured" });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("FilmTypeManagement/GetAllFilmTypes: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("GetFilmTypeByID/{typeID}")]
        public IActionResult GetFilmTypeByName([FromServices] IFilmTypeService filmTypeService, int typeID)
        {
            try
            {
                FilmType filmType = filmTypeService.GetFilmTypeByID(typeID);
                if (filmType != null) return new JsonResult(filmType);
                return new JsonResult(new { Message = "An error occured" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("FilmTypeManagement/GetFilmTypeByID: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("RemoveFilmType/{filmTypeName}")]
        public IActionResult RemoveFilmType([FromServices] IFilmTypeService filmTypeService, string filmTypeName)
        {
            try
            {
                bool result = filmTypeService.RemoveFilmType(filmTypeName);
                if (result) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("FilmTypeManagement/RemoveFilmType: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
    }
}
