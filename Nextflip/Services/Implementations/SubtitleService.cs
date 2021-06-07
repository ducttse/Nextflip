using Nextflip.Services.Interfaces;
using Nextflip.Models.subtitle;
using System.Collections.Generic;

namespace Nextflip.Services.Implementations
{
    public class SubtitleService : ISubtitleService
    {
        private readonly ISubtitleDAO _subtitleDAO; 
        public SubtitleService(ISubtitleDAO subtitleDAO) => _subtitleDAO = subtitleDAO;
        public IEnumerable<SubtitleDTO> GetSubtitlesByEpisodeID(string episodeID) => _subtitleDAO.GetSubtitlesByEpisodeID(episodeID);
    }
}
