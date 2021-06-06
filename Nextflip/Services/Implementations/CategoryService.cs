
using Nextflip.Models.category;
using Nextflip.Models.mediaCategory;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        public ICategoryDAO CategoryDAO { get; }
        public IMediaCategoryDAO MediaCategoryDAO { get;}
        public CategoryService (ICategoryDAO categoryDAO, IMediaCategoryDAO mediaCategoryDAO)
        {
            CategoryDAO = categoryDAO;
            MediaCategoryDAO = mediaCategoryDAO;
        }
        public IEnumerable<CategoryDTO> GetCategories() => CategoryDAO.GetCategories();

        public IEnumerable<CategoryDTO> GetCategoriesByMediaID(string mediaID)
        {
            var categories = new List<CategoryDTO>();
            IList<int> categoryIDs =   MediaCategoryDAO.GetCategoryIDs(mediaID);
            foreach(var categoryID in categoryIDs)
            {
                CategoryDTO category = CategoryDAO.GetCategoryByID(categoryID);
                categories.Add(category);
            }
            return categories;
        }
    }
}
