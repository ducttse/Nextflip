using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;

namespace Nextflip.Services.Interfaces
{
    public interface IEditorService
    {
        IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearching(string searchValue);

        IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName);

        IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status);

        IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory(string CategoryName);
        IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory_Status(string CategoryName, string Status);
        bool RequestDisableMedia(string mediaID);
        Media GetMediaByID(string mediaID);
        IEnumerable<Media> GetAllMedia(int RowsOnPage, int RequestPage);
        int NumberOfMedias();
        IEnumerable<Media> GetAllMediaFilterStatus(string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterStatus(string Status);
        IEnumerable<Media> GetMediasByTitleFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterStatus(string searchValue, string Status);
        bool RequestChangeMediaStatus(string mediaID, string newStatus);
        bool RequestChangeEpisodeStatus(string episodeID, string newStatus);
        Media GetMediaByChildID(string childID, string type);
        bool RequestChangeSeasonStatus(string seasonID, string newStatus);
   
        string AddMedia(string Title, string FilmType, string Director, string Cast, int? PublishYear,
            string Duration, string BannerURL, string Language, string Description);
        // category
        Category GetCategoryById(int categoryID);
        bool AddMediaCategory(string mediaID, int categoryID);
        //add media
        string UpdateMedia(Media media);
        //season
        string AddSeason(Season season);
        string UpdateSeason(Season season);
        //episode
        string AddEpisode(Episode episode);
        string UpdateEpisode(Episode episode);
        string AddNewMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm);
        string EditMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm);
    }
}
