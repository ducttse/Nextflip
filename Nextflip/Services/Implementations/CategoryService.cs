using Nextflip.Models.DTO;
using Nextflip.Models.DAO;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        public IEnumerable<Category> GetCategories() => CategoryDAO.Instance.GetCategories();

        public IEnumerable<Category> GetCategoriesByMediaID(string mediaID)
        {
            var categories = new List<Category>();
            List<int> categoryIDs = MediaCategoryDAO.Instance.GetCategoryIDs(mediaID);
            foreach(var categoryID in categoryIDs)
            {
                Category category = CategoryDAO.Instance.GetCategoryByID(categoryID);
                categories.Add(category);
            }
            return categories;
        }
    }
}
