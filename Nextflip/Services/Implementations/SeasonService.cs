using Nextflip.Models.DTO;
using Nextflip.Models.DAO;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class SeasonService : ISeasonService
    {
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID) => SeasonDAO.Instance.GetSeasonsByMediaID(mediaID);
    }
}
