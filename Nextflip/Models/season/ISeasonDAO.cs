using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.season
{
    public interface ISeasonDAO
    {
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID);
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID, string status);
        Season GetSeasonByID(string seasonID);
        bool ApproveChangeSeason(string ID);
        bool DisapproveChangeSeason(string ID, string note);
        bool RequestChangeSeasonStatus(string seasonID, string newStatus);
        string AddSeason(Season season);
        string UpdateSeason(Season season);
        public bool CheckStatusSeason(string mediaID);
        int NumberAvailableSeason(string mediaID);
    }
}
