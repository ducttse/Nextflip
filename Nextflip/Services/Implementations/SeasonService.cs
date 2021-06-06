using Nextflip.Services.Interfaces;
using System.Collections.Generic;
using Nextflip.Models.season;

namespace Nextflip.Services.Implementations
{
    public class SeasonService : ISeasonService
    {
        public SeasonService(ISeasonDAO seasonDAO)
        {
            SeasonDAO = seasonDAO;
        }
        public ISeasonDAO SeasonDAO { get; }
        public IEnumerable<SeasonDTO> GetSeasonsByMediaID(string mediaID) => SeasonDAO.GetSeasonsByMediaID(mediaID);
    }
}
