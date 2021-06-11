using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    {
        Media GetMediaByID(string mediaID);
        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMedias();
        IEnumerable<Media> GetMediasByTitle(string title);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID);
    }
}
