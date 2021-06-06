using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaFavorite
{
    public interface IMediaFavoriteDAO
    {
        IList<string> GetMediaIDs(string favoriteListID);
    }
}
