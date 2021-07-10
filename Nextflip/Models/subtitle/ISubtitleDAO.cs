using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nextflip.Models.subtitle
{
    public interface ISubtitleDAO
    {
        IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID);
        Subtitle GetSubtitleByID(string subtitleID);
        bool ApproveChangeSubtitle(string ID);
        bool DisapproveChangeSubtitle(string ID);
        bool RequestChangeSubtitleStatus(string subtitleID, string newStatus);
    }
}
