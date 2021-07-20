using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;


namespace Nextflip.Services.Interfaces
{
    public interface IMediaManagerManagementService
    {
      
        bool ApproveChangeMedia(string mediaID);
        bool DisapproveChangeMedia(string mediaID);
        Media GetMediaByID(string mediaID);
        bool ApproveChangeEpisode(string ID);
        bool DisapproveChangeEpisode(string ID);
        bool ApproveChangeSeason(string ID);
        bool DisapproveChangeSeason(string ID);
        Season GetSeasonByID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
        ViewEditorDashboard.PrototypeMediaForm GetDetailedMediaByMediaId(string mediaId);
    }
}
