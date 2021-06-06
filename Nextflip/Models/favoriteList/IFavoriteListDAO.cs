using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.favoriteList
{
    public interface IFavoriteListDAO
    {
        FavoriteListDTO GetFavoriteList(string userID);
    }
}
