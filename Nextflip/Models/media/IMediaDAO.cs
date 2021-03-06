using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;

namespace Nextflip.Models.media
{
    public interface IMediaDAO
    {
        IEnumerable<Media> GetMediasByTitle(string searchValue);
        IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearching(string searchValue);

        IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName);

        IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status);

        Media GetMediaByID(string mediaID);

        IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory(string CategoryName);

        IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory_Status(string CategoryName, string Status);

        bool RequestDisableMedia(string mediaID);
        bool ApproveChangeMedia(string mediaID);
        bool DisapproveChangeMedia(string mediaID, string note);
        IEnumerable<Media> GetAllMedia(int RowsOnPage, int RequestPage);
        int NumberOfMedias();
        IEnumerable<Media> GetAllMediaFilterStatus(string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterStatus(string Status);
        IEnumerable<Media> GetMediasByTitleFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterStatus(string searchValue, string Status);
        bool RequestChangeMediaStatus(string mediaID, string newStatus, string note);
        Media GetMediaByChildID(string childID, string type);
        IEnumerable<Media> GetNewestMedias(int limit);
        string AddMedia(string Title, string FilmType, string Director, string Cast, int? PublishYear, 
            string Duration, string BannerURL, string Language, string Description);
        string UpdateMedia(Media media);
        string AddMedia(Media mediaInfo);
        string AddNewMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm);
        ViewEditorDashboard.PrototypeMediaForm GetDetailedMedia(string mediaId);
        public string CloneMedia(string mediaID);
        string EditMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm);
        IEnumerable<Media> EditorViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int EditorNumberOfMediasFilterCategory_Status(string CategoryName, string Status);
        IEnumerable<Media> EditorGetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int EditorNumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status);

    }
}
