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
        Media GetMediaByID(string mediaID);

        IEnumerable<Media> GetMedias(int RowsOnPage, int RequestPage);
        int NumberOfMedias();
    }
}
