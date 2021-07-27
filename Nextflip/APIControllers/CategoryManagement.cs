using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryManagement : ControllerBase
    {
        private readonly ILogger _logger;
        public CategoryManagement(ILogger<CategoryManagement> logger)
        {
            _logger = logger;
        }

        [Route("UpdateCategory/{categoryID}/{categoryName}")]
        public IActionResult UpdateCategory([FromServices] ICategoryService categoryService, int categoryID, string categoryName)
        {
            try
            {
                bool result = categoryService.UpdateCategory(categoryID, categoryName);
                if(result) return new JsonResult(new {Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("CategoryManagement/UpdateCategory: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("CreateNewCategory/{categoryName}")]
        public IActionResult CreateNewCategory([FromServices] ICategoryService categoryService, string categoryName)
        {
            try
            {
                bool result = categoryService.CreateNewCategory(categoryName);
                if (result) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoryManagement/CreateNewCategory: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("RemoveCategory/{categoryName}")]
        public IActionResult RemoveCategory([FromServices] ICategoryService categoryService, string categoryName)
        {
            try
            {
                bool result = categoryService.RemoveCategory(categoryName);
                if (result) return new JsonResult(new { Message = "Success" });
                return new JsonResult(new { Message = "Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoryManagement/RemoveCategory: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
    }
}
