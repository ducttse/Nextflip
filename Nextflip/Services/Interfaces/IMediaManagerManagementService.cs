using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.mediaEditRequest;
using Nextflip.Models.season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nextflip.Services.Interfaces
{
    public interface IMediaManagerManagementService
    {
        IEnumerable<MediaEditRequest> GetAllPendingMedias(); 

        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmailFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage);
        int NumberOfPendingMediasBySearchingFilterStatus(string searchValue, string Status);

        bool ChangeRequestStatus(int requestID, string status);
        bool ApproveRequest(int requestID);
        bool DisappoveRequest(int requestID, string note);

        int NumberOfPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage);

        int NumberOfPendingMediasFilterStatus(string status);
        IEnumerable<MediaEditRequest> GetPendingMediasFilterStatus(string status, int RowsOnPage, int RequestPage);
        bool ApproveChangeMedia(string mediaID);
        bool DisapproveChangeMedia(string mediaID);
        IEnumerable<MediaEditRequest> GetMediaRequest(string status, string type, string sortBy, int RowsOnPage, int RequestPage);
        int NumberOfMediaRequest(string status, string type);
        IEnumerable<MediaEditRequest> SearchingMediaRequest(string searchValue, string status, string sortBy, string type, int RowsOnPage, int RequestPage);
        int NumberOfMediaRequestSearching(string searchValue, string status, string type);
        Media GetMediaByID(string mediaID);
        MediaEditRequest GetMediaEditRequestByID(int requestID);
        bool ApproveChangeEpisode(string ID);
        bool DisapproveChangeEpisode(string ID);
        bool ApproveChangeSeason(string ID);
        bool DisapproveChangeSeason(string ID);
        bool ApproveChangeSubtitle(string ID);
        bool DisapproveChangeSubtitle(string ID);
        Season GetSeasonByID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
    }
}
