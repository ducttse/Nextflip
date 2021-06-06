using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaFavorite;
using Nextflip.Services.Interfaces;
using System.Collections.Generic;

namespace Nextflip.Services.Implementations
{
    public class MediaService : IMediaService
    {
        public IFavoriteListDAO FavoriteListDAO { get;  }
        public IMediaDAO MediaDAO { get; }
        public IMediaFavoriteDAO MediaFavoriteDAO { get; }
        public MediaService( IFavoriteListDAO favoriteListDAO, IMediaDAO mediaDAO, IMediaFavoriteDAO mediaFavoriteDAO)
        {
            FavoriteListDAO = favoriteListDAO;
            MediaDAO = mediaDAO;
            MediaFavoriteDAO = mediaFavoriteDAO;
        }

        public IEnumerable<MediaDTO> GetFavoriteMediasByUserID(string userID)
        {
            var favoriteMedias = new List<MediaDTO>();
            FavoriteListDTO favoriteList = FavoriteListDAO.GetFavoriteList(userID);
            IList<string> favoriteMediaIDs = MediaFavoriteDAO.GetMediaIDs(favoriteList.FavoriteListID);
            foreach (string mediaID in favoriteMediaIDs)
            {
                favoriteMedias.Add(MediaDAO.GetMediasByID(mediaID));
            }

            return favoriteMedias;
        }

        public IEnumerable<MediaDTO> GetMediasByTitle(string title) => MediaDAO.GetMediasByTitle(title);
    }
}
