using Nextflip.Models.subtitle;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface ISubtitleService
    {
        Subtitle GetSubtitleByID(string subtitleID);
        IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID);
    }
}
