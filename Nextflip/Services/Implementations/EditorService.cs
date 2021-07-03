using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.mediaEditRequest;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class EditorService : IEditorService
    {
        private readonly IMediaDAO _mediaDAO;
        private readonly IMediaEditRequestDAO _mediaEditRequestDAO;
        private readonly IEpisodeDAO _episodeDAO;
        public EditorService(IMediaEditRequestDAO mediaEditRequestDAO, IMediaDAO mediaDAO, IEpisodeDAO episodeDAO)
        {
            _mediaEditRequestDAO = mediaEditRequestDAO;
            _mediaDAO = mediaDAO;
            _episodeDAO = episodeDAO;
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

        public IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage)
             => _mediaDAO.GetMediasByTitleFilterCategory_Status(SearchValue, CategoryName, Status, RowsOnPage, RequestPage);

        public int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status)
            => _mediaDAO.NumberOfMediasBySearchingFilterCategory_Status(SearchValue, CategoryName, Status);

        public bool RequestDisableMedia(string mediaID) => _mediaDAO.RequestDisableMedia(mediaID);
        public Media GetMediaByID(string mediaID) => _mediaDAO.GetMediaByID(mediaID);
        public bool AddMediaRequest(string userEmail, string mediaID, string note, string previewLink, string type, string ID)
                => _mediaEditRequestDAO.AddMediaRequest(userEmail, mediaID, note, previewLink, type, ID);
        public IEnumerable<Media> GetAllMedia(int RowsOnPage, int RequestPage) => _mediaDAO.GetAllMedia(RowsOnPage, RequestPage);
        public int NumberOfMedias() => _mediaDAO.NumberOfMedias();
        public IEnumerable<Media> GetAllMediaFilterStatus(string Status, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetAllMediaFilterStatus(Status, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterStatus(string Status) => _mediaDAO.NumberOfMediasFilterStatus(Status);
        public IEnumerable<Media> GetMediasByTitleFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetMediasByTitleFilterStatus(searchValue, Status, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearchingFilterStatus(string searchValue, string Status)
            => _mediaDAO.NumberOfMediasBySearchingFilterStatus(searchValue, Status);
        public bool RequestChangeMediaStatus(string mediaID, string newStatus)
            => _mediaDAO.RequestChangeMediaStatus(mediaID, newStatus);
        public bool RequestChangeEpisodeStatus(string episodeID, string newStatus)
            => _episodeDAO.RequestChangeEpisodeStatus(episodeID, newStatus);
        public  Media GetMediaByChildID(string childID, string type)
            => _mediaDAO.GetMediaByChildID(childID, type);
    }
}
