using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.favoriteList
{
    public interface IFavoriteListDAO
    {
        FavoriteList GetFavoriteList(string userID);
        void AddNewFavoriteList(string userID);
    }
}
