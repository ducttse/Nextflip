using System.Collections.Generic;
using Nextflip.Models.episode;
using Nextflip.Services.Interfaces;


namespace Nextflip.Services.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IEpisodeDAO _episodeDAO ;
        public EpisodeService(IEpisodeDAO episodeDAO) =>_episodeDAO = episodeDAO;

        public Episode GetEpisodeByID(string episodeID) => _episodeDAO.GetEpisodeByID(episodeID);

        IEnumerable<Episode> IEpisodeService.GetEpisodesBySeasonID(string seasonID) 
                                            => _episodeDAO.GetEpisodesBySeasonID(seasonID);

        
    }
}
