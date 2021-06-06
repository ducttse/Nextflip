using Nextflip.Models.DAO;
using Nextflip.Models.DTO;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class MediaService : IMediaService
    {
        IEnumerable<Media> IMediaService.GetFavoriteMediasByUserID(string userID)
        {
            var favoriteMedias = new List<Media>();
            FavoriteList favoriteList = FavoriteListDAO.Instance.GetFavoriteList(userID);
            List<string> favoriteMediaIDs = MediaFavoriteDAO.Instance.GetMediaIDs(favoriteList.FavoriteListID);
            foreach (string mediaID in favoriteMediaIDs)
            {
                favoriteMedias.Add(MediaDAO.Instance.GetMediasByID(mediaID));
            }

            return favoriteMedias;
        }

        IEnumerable<Media> IMediaService.GetMediasByTitle(string title) => MediaDAO.Instance.GetMediasByTitle(title);
    }
}
