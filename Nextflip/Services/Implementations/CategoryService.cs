using Nextflip.Models.category;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAO _categoryDAO;
        public CategoryService(ICategoryDAO categoryDAO) => _categoryDAO = categoryDAO;
        public bool UpdateCategory(int categoryID, string newCategoryName) => _categoryDAO.UpdateCategory(categoryID, newCategoryName);
        public bool CreateNewCategory(string categoryName) => _categoryDAO.CreateNewCategory(categoryName);
    }
}
