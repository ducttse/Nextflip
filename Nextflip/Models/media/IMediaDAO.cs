using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public interface IMediaDAO
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
        bool ChangeMediaStatus(string mediaID, string status);
    }
}
