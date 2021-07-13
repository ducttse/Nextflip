using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ICategoryService
    {
        public bool UpdateCategory(int categoryID, string newCategoryName);
        public bool CreateNewCategory(string categoryName);
    }
}
