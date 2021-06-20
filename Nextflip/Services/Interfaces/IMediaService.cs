using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearching(string searchValue);

        IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, int CategoryID, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory(string SearchValue, int CategoryID);

        IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, int CategoryID, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, int CategoryID, string Status);

        Media GetMediaByID(string mediaID);
        IEnumerable<Media> GetMediaFilterCategory(int CategoryID, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory(int CategoryID);
        IEnumerable<Media> ViewMediasFilterCategory_Status(int CategoryID, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory_Status(int CategoryID, string Status);

        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID);

       // bool ChangeMediaStatus(string mediaID);
    }
}
