using Nextflip.Models.DTO;
using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaFavorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IMediaService
    { 
        IEnumerable<MediaDTO> GetFavoriteMediasByUserID(string userID);
        IEnumerable<MediaDTO> GetMediasByTitle(string title);
    }
}
