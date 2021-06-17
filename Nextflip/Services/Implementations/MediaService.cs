﻿using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaCategory;
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
        private readonly IMediaCategoryDAO _mediaCategoryDAO;
        public MediaService( IFavoriteListDAO favoriteListDAO, IMediaDAO mediaDAO, 
                            IMediaFavoriteDAO mediaFavoriteDAO, IMediaCategoryDAO mediaCategoryDAO)
        {
            _favoriteListDAO = favoriteListDAO;
            _mediaDAO = mediaDAO;
            _mediaFavoriteDAO = mediaFavoriteDAO;
            _mediaCategoryDAO = mediaCategoryDAO;
        }

        public Media GetMediaByID(string mediaID) => _mediaDAO.GetMediaByID(mediaID);

        public IEnumerable<Media> GetFavoriteMediasByUserID(string userID)
        {
            var favoriteMedias = new List<Media>();
            FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
            IList<string> favoriteMediaIDs = _mediaFavoriteDAO.GetMediaIDs(favoriteList.FavoriteListID);
            foreach (string mediaID in favoriteMediaIDs)
            {
                favoriteMedias.Add(_mediaDAO.GetMediaByID(mediaID));
            }
            return favoriteMedias;
        }
        public IEnumerable<Media> GetMediasByTitle(string title) => _mediaDAO.GetMediasByTitle(title);
        public IEnumerable<Media> GetMediasByCategoryID(int categoryID)
        {
            var medias = new List<Media>();
            IList<string> mediaIDs = _mediaCategoryDAO.GetMediaIDs(categoryID);
            foreach (var mediaID in mediaIDs)
            {
                Media media = _mediaDAO.GetMediaByID(mediaID);
                medias.Add(media);
            }
            return medias;
        }
    }
}
