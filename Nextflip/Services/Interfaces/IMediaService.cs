using Nextflip.Models.media;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    { 
        IEnumerable<MediaDTO> GetFavoriteMediasByUserID(string userID);
        IEnumerable<MediaDTO> GetMediasByTitle(string title);
    }
}
