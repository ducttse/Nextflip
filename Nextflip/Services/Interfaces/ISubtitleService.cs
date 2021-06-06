using Nextflip.Models.subtitle;

namespace Nextflip.Services.Interfaces
{
    public interface ISubtitleService
    {
        SubtitleDTO GetSubtitleByEpisodeID(string episodeID);
    }
}
