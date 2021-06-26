using Nextflip.Models.media;
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
        public EditorService(IMediaDAO mediaDAO) =>  _mediaDAO = mediaDAO;
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
    }
}
