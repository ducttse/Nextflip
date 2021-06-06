using Nextflip.Models.season;
using System.Collections.Generic;

namespace Nextflip.Services.Interfaces
{
    public interface ISeasonService
    {
        IEnumerable<SeasonDTO> GetSeasonsByMediaID(string mediaID);
    }
}
