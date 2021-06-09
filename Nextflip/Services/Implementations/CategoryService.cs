﻿
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
        private readonly ICategoryDAO _categoryDAO;
        private readonly IMediaCategoryDAO _mediaCategoryDAO;
        public CategoryService (ICategoryDAO categoryDAO, IMediaCategoryDAO mediaCategoryDAO)
        {
            _categoryDAO = categoryDAO;
            _mediaCategoryDAO = mediaCategoryDAO;
        }
        public IEnumerable<Category> GetCategories() => _categoryDAO.GetCategories();

        public IEnumerable<Category> GetCategoriesByMediaID(string mediaID)
        {
            var categories = new List<Category>();
            IList<int> categoryIDs =   _mediaCategoryDAO.GetCategoryIDs(mediaID);
            foreach(var categoryID in categoryIDs)
            {
                Category category = _categoryDAO.GetCategoryByID(categoryID);
                categories.Add(category);
            }
            return categories;
        }
    }
}