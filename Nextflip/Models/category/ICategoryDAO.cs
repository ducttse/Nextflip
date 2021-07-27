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
        public bool UpdateCategory(int categoryID, string newCategoryName);
        public bool CreateNewCategory(string categoryName);
        Category GetCategoryByName(string categoryName);
        public bool RemoveCategory(string categoryName);
    }
}
