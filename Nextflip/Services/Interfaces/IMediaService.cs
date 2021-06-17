using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearching(string searchValue);
        Media GetMediaByID(string mediaID);
        IEnumerable<Media> GetMedias(int RowsOnPage, int RequestPage);
        int NumberOfMedias();
        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID);
    }
}
