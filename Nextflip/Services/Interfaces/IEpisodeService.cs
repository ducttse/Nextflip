using Nextflip.Models.episode;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface IEpisodeService
    {
        IEnumerable<EpisodeDTO> GetEpisodesBySeasonID(string seasonID);
    }
}
