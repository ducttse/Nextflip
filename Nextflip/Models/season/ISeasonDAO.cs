using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.season
{
    public interface ISeasonDAO
    {
        IEnumerable<SeasonDTO> GetSeasonsByMediaID(string mediaID);
    }
}
