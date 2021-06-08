using Nextflip.Models.episode;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IEpisodeService
    {
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
    }
}
