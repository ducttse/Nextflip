using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaFavorite;
using Nextflip.Services.Interfaces;
using System.Collections.Generic;

namespace Nextflip.Services.Implementations
{
    public class MediaService : IMediaService
    {
        private readonly IFavoriteListDAO _favoriteListDAO;
        private readonly IMediaDAO _mediaDAO;
        private readonly IMediaFavoriteDAO _mediaFavoriteDAO; 
        public MediaService( IFavoriteListDAO favoriteListDAO, IMediaDAO mediaDAO, IMediaFavoriteDAO mediaFavoriteDAO)
        {
            _favoriteListDAO = favoriteListDAO;
            _mediaDAO = mediaDAO;
            _mediaFavoriteDAO = mediaFavoriteDAO;
        }

        public IEnumerable<MediaDTO> GetFavoriteMediasByUserID(string userID)
        {
            var favoriteMedias = new List<MediaDTO>();
            FavoriteListDTO favoriteList = _favoriteListDAO.GetFavoriteList(userID);
            IList<string> favoriteMediaIDs = _mediaFavoriteDAO.GetMediaIDs(favoriteList.FavoriteListID);
            foreach (string mediaID in favoriteMediaIDs)
            {
                favoriteMedias.Add(_mediaDAO.GetMediasByID(mediaID));
            }

            return favoriteMedias;
        }

        public IEnumerable<MediaDTO> GetMediasByTitle(string title) => _mediaDAO.GetMediasByTitle(title);
    }
}
