using Nextflip.Services.Interfaces;
using System.Collections.Generic;
using Nextflip.Models.season;

namespace Nextflip.Services.Implementations
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonDAO _seasonDAO; 
        public SeasonService(ISeasonDAO seasonDAO) => _seasonDAO = seasonDAO;

        public Season GetSeasonByID(string seasonID) => _seasonDAO.GetSeasonByID(seasonID);
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID) => _seasonDAO.GetSeasonsByMediaID(mediaID);
    }
}
