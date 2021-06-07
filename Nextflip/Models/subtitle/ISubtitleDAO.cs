using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nextflip.Models.subtitle
{
    public interface ISubtitleDAO
    {
        IEnumerable<SubtitleDTO> GetSubtitlesByEpisodeID(string episodeID);
    }
}
