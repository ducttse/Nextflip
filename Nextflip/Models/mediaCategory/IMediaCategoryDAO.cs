using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaCategory
{
    public interface IMediaCategoryDAO
    {
        Task<IList<int>> GetCategoryIDs(string mediaID);
        Task<IList<string>> GetMediaIDs(int categoryID);
    }
}
