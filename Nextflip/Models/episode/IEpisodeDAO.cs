using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public interface IEpisodeDAO
    {
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID);
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID,string status);
        Episode GetEpisodeByID(string episodeID);
        bool ApproveChangeEpisode(string ID);
        bool DisapproveChangeEpisode(string ID, string note);
        bool RequestChangeEpisodeStatus(string episodeID, string newStatus);
        string AddEpisode(Episode episode);
        string UpdateEpisode(Episode episode);
        public bool CheckStatusEpisode(string seasonID);
    }
}
