using Nextflip.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISeasonService
    {
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID);
    }
}
