using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    {
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

        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID);

        bool RequestDisableMedia(string mediaID);
    }
}
