using Nextflip.Services.Interfaces;
using Nextflip.Models.subtitle;

namespace Nextflip.Services.Implementations
{
    public class SubtitleService : ISubtitleService
    {
        private readonly ISubtitleDAO _subtitleDAO; 
        public SubtitleService(ISubtitleDAO subtitleDAO) => _subtitleDAO = subtitleDAO;
        public SubtitleDTO GetSubtitleByEpisodeID(string episodeID) => _subtitleDAO.GetSubtitleByEpisodeID(episodeID);
    }
}
