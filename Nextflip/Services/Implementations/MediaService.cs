using Nextflip.Models.favoriteList;
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
            favoriteMedias.Reverse();
            return favoriteMedias;
        }

        public IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage) => _mediaDAO.GetMediasByTitle(searchValue, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearching(string searchValue) => _mediaDAO.NumberOfMediasBySearching(searchValue);

        public IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetMediasByTitleFilterCategory(SearchValue, CategoryName, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName)
            => _mediaDAO.NumberOfMediasBySearchingFilterCategory(SearchValue, CategoryName);

        public IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage) => _mediaDAO.GetMediaFilterCategory(CategoryName, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterCategory(string CategoryName) => _mediaDAO.NumberOfMediasFilterCategory(CategoryName);

        public IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage) => _mediaDAO.ViewMediasFilterCategory_Status(CategoryName, Status, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterCategory_Status(string CategoryName, string Status) => _mediaDAO.NumberOfMediasFilterCategory_Status(CategoryName, Status);
        
        
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

        public IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage)
             => _mediaDAO.GetMediasByTitleFilterCategory_Status(SearchValue, CategoryName, Status, RowsOnPage, RequestPage);

        public int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status)
            => _mediaDAO.NumberOfMediasBySearchingFilterCategory_Status(SearchValue, CategoryName, Status);

        public bool ChangeMediaStatus(string mediaID, string status) => _mediaDAO.ChangeMediaStatus(mediaID,status);
    }
}
