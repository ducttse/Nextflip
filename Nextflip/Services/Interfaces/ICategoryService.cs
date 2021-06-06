using Nextflip.Models.category;
using Nextflip.Models.mediaCategory;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDTO> GetCategories();

        IEnumerable<CategoryDTO> GetCategoriesByMediaID(string mediaID);
    }
}
