using Nextflip.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISubtitleService
    {
        Subtitle GetSubtitleByEpisodeID(string episodeID);
    }
}
