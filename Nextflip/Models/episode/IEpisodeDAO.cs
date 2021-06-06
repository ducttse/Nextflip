﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public interface IEpisodeDAO
    {
        IEnumerable<EpisodeDTO> GetEpisodesBySeasonID(string seasonID);
    }
}