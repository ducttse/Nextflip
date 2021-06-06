using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.favoriteList
{
    public interface IFavoriteListDAO
    {
        Task<FavoriteListDTO> GetFavoriteList(string userID);
    }
}
