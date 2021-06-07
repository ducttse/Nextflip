using Nextflip.Models.subtitle;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface ISubtitleService
    {
        IEnumerable<SubtitleDTO> GetSubtitlesByEpisodeID(string episodeID);
    }
}
