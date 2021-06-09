﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public interface IMediaDAO
    {
        IEnumerable<Media> GetMediasByTitle(string searchValue);
        Media GetMediaByID(string mediaID);
    }
}