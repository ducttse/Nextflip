using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.category
{
    public interface ICategoryDAO
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByID(int categoryID);
        Category GetCategoryByName(string categoryName);
    }
}
