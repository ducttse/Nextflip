using Nextflip.Services.Interfaces;
using Nextflip.Models.subtitle;

namespace Nextflip.Services.Implementations
{
    public class SubtitleService : ISubtitleService
    {
        public SubtitleService(ISubtitleDAO subtitleDAO)
        {
            SubtitleDAO = subtitleDAO;
        }
        public ISubtitleDAO SubtitleDAO { get;  }
        public SubtitleDTO GetSubtitleByEpisodeID(string episodeID) 
                                                        =>  SubtitleDAO.GetSubtitleByEpisodeID(episodeID);
    }
}
