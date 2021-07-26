using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.mediaCategory;
using Nextflip.Models.season;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;

namespace Nextflip.Services.Implementations
{
    public class EditorService : IEditorService
    {
        private readonly IMediaDAO _mediaDAO;
        private readonly IEpisodeDAO _episodeDAO;
        private readonly ISeasonDAO _seasonDAO;
        private readonly ICategoryDAO _categoryDAO;
        private readonly IMediaCategoryDAO _mediaCategoryDAO;
        public EditorService(IMediaDAO mediaDAO, IEpisodeDAO episodeDAO, ISeasonDAO seasonDAO, 
            IMediaCategoryDAO mediaCategoryDAO, ICategoryDAO categoryDAO)
        {
            _mediaDAO = mediaDAO;
            _episodeDAO = episodeDAO;
            _seasonDAO = seasonDAO;
            _mediaCategoryDAO = mediaCategoryDAO;
            _categoryDAO = categoryDAO;
        }
        public IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage) => _mediaDAO.GetMediasByTitle(searchValue, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearching(string searchValue) => _mediaDAO.NumberOfMediasBySearching(searchValue);

        public IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetMediasByTitleFilterCategory(SearchValue, CategoryName, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName)
            => _mediaDAO.NumberOfMediasBySearchingFilterCategory(SearchValue, CategoryName);

        public IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage) => _mediaDAO.GetMediaFilterCategory(CategoryName, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterCategory(string CategoryName) => _mediaDAO.NumberOfMediasFilterCategory(CategoryName);

        public IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage) => _mediaDAO.EditorViewMediasFilterCategory_Status(CategoryName, Status, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterCategory_Status(string CategoryName, string Status) => _mediaDAO.EditorNumberOfMediasFilterCategory_Status(CategoryName, Status);

        public IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage)
             => _mediaDAO.EditorGetMediasByTitleFilterCategory_Status(SearchValue, CategoryName, Status, RowsOnPage, RequestPage);

        public int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status)
            => _mediaDAO.EditorNumberOfMediasBySearchingFilterCategory_Status(SearchValue, CategoryName, Status);

        public bool RequestDisableMedia(string mediaID) => _mediaDAO.RequestDisableMedia(mediaID);
        public Media GetMediaByID(string mediaID) => _mediaDAO.GetMediaByID(mediaID);
        public IEnumerable<Media> GetAllMedia(int RowsOnPage, int RequestPage) => _mediaDAO.GetAllMedia(RowsOnPage, RequestPage);
        public int NumberOfMedias() => _mediaDAO.NumberOfMedias();
        public IEnumerable<Media> GetAllMediaFilterStatus(string Status, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetAllMediaFilterStatus(Status, RowsOnPage, RequestPage);
        public int NumberOfMediasFilterStatus(string Status) => _mediaDAO.NumberOfMediasFilterStatus(Status);
        public IEnumerable<Media> GetMediasByTitleFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage)
            => _mediaDAO.GetMediasByTitleFilterStatus(searchValue, Status, RowsOnPage, RequestPage);
        public int NumberOfMediasBySearchingFilterStatus(string searchValue, string Status)
            => _mediaDAO.NumberOfMediasBySearchingFilterStatus(searchValue, Status);
        public bool RequestChangeMediaStatus(string mediaID, string newStatus, string note)
            => _mediaDAO.RequestChangeMediaStatus(mediaID, newStatus, note);
        public bool RequestChangeEpisodeStatus(string episodeID, string newStatus)
            => _episodeDAO.RequestChangeEpisodeStatus(episodeID, newStatus);
        public  Media GetMediaByChildID(string childID, string type)
            => _mediaDAO.GetMediaByChildID(childID, type);
        public bool RequestChangeSeasonStatus(string seasonID, string newStatus)
            => _seasonDAO.RequestChangeSeasonStatus(seasonID, newStatus);
        public string AddMedia(string Title, string FilmType, string Director, string Cast, int? PublishYear,
            string Duration, string BannerURL, string Language, string Description)
            => _mediaDAO.AddMedia(Title, FilmType, Director, Cast, PublishYear, Duration, BannerURL, Language, Description);

        public Category GetCategoryById(int categoryID) => _categoryDAO.GetCategoryByID(categoryID);
        public bool AddMediaCategory(string mediaID, int categoryID) => _mediaCategoryDAO.AddMediaCategory(mediaID, categoryID);
        public string UpdateMedia(Media media) => _mediaDAO.UpdateMedia(media);

        public string AddSeason(Season season) => _seasonDAO.AddSeason(season);

        public string UpdateSeason(Season season) => _seasonDAO.UpdateSeason(season);


        public string AddEpisode(Episode episode) => _episodeDAO.AddEpisode(episode);

        public string UpdateEpisode(Episode episode) => _episodeDAO.UpdateEpisode(episode);
        public string AddNewMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm)
        {
            return _mediaDAO.AddNewMedia(mediaForm);
        }

        public string EditMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm)
        {
            return _mediaDAO.EditMedia(mediaForm);
        }

        public int NumberAvailableSeason(string mediaID) => _seasonDAO.NumberAvailableSeason(mediaID);
    }
}
