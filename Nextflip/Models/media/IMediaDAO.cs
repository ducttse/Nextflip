using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public interface IMediaDAO
    {
        Task<IEnumerable<MediaDTO>> GetMediasByTitle(string searchValue);
        Task<MediaDTO> GetMediasByID(string mediaID);
    }
}
