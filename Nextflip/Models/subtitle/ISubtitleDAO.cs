using System.Threading.Tasks;

namespace Nextflip.Models.subtitle
{
    public interface ISubtitleDAO
    {
        Task<SubtitleDTO> GetSubtitleByEpisodeID(string episodeID);
    }
}
