﻿using Nextflip.Models.DTO;
using Nextflip.Models.episode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IEpisodeService
    {
        IEnumerable<EpisodeDTO> GetEpisodesBySeasonID(string seasonID);
    }
}
