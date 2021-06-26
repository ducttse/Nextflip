using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        bool ChangeMediaStatus(string mediaID, string status);
    }
}
