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
        IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory_Status(string CategoryName, string Status);
        IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status);
        public bool CheckStatusEpisode(string seasonID);
        public bool CheckStatusSeason(string mediaID);
        int NumberAvailableSeason(string mediaID);
    }
}
