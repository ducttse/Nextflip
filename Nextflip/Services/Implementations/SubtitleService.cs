using Nextflip.Models.DTO;
using Nextflip.Models.DAO;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class SubtitleService : ISubtitleService
    {
        public Subtitle GetSubtitleByEpisodeID(string episodeID) => SubtitleDAO.Instance.GetSubtitleByEpisodeID(episodeID);
    }
}
