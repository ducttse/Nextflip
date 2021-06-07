using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    { 
        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMediasByTitle(string title);

        IEnumerable<Media> GetMediasByCategoryID(int categoryID);
    }
}
