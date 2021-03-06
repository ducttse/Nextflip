using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaCategory
{
    public interface IMediaCategoryDAO
    {
        IList<int> GetCategoryIDs(string mediaID);
        bool AddMediaCategory(string mediaID, int categoryID);
        IList<string> GetMediaIDs(int categoryID, int limit);
    }
}
