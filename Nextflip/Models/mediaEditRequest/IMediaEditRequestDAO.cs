using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaEditRequest
{
    public interface IMediaEditRequestDAO
    {
        IEnumerable<MediaEditRequest> GetAllPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfPendingMediasBySearching(string searchValue);

        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmailFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage);
        int NumberOfPendingMediasBySearchingFilterStatus(string searchValue, string Status);

        bool ChangeRequestStatus(int requestID, string status);
        bool ApproveRequest(int requestID);
        bool DisapproveRequest(int requestID, string note);

        int NumberOfPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage);


        int NumberOfPendingMediasFilterStatus(string status);
        IEnumerable<MediaEditRequest> GetPendingMediasFilterStatus(string status, int RowsOnPage, int RequestPage);

        bool AddMediaRequest(string userEmail, string mediaID, string note, string previewLink);
        MediaEditRequest GetMediaEditRequestByID(int requestID);
    }
}
