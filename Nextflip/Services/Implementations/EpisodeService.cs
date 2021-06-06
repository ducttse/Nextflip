using System.Collections.Generic;
using Nextflip.Models.episode;
using Nextflip.Services.Interfaces;


namespace Nextflip.Services.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        public IEpisodeDAO EpisodeDAO { get; }
        public EpisodeService(IEpisodeDAO episodeDAO)
        {
            EpisodeDAO = episodeDAO;
        }
        IEnumerable<EpisodeDTO> IEpisodeService.GetEpisodesBySeasonID(string seasonID) 
                                            => EpisodeDAO.GetEpisodesBySeasonID(seasonID);

        
    }
}
