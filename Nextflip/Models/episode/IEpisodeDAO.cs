using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public interface IEpisodeDAO
    {
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
        bool ApproveChangeEpisode(string ID);
        bool DisapproveChangeEpisode(string ID);
        bool RequestChangeEpisodeStatus(string episodeID, string newStatus);
    }
}
