using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.DAO;
using Nextflip.Models.DTO;
using Nextflip.Services.Interfaces;


namespace Nextflip.Services.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        IEnumerable<Episode> IEpisodeService.GetEpisodesBySeasonID(string seasonID) 
                                            => EpisodeDAO.Instance.GetEpisodesBySeasonID(seasonID);

        
    }
}
