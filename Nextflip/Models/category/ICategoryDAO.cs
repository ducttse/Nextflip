using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.category
{
    public interface ICategoryDAO
    {
        IEnumerable<CategoryDTO> GetCategories();
        CategoryDTO GetCategoryByID(int categoryID);
    }
}
