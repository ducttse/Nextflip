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
        bool ChangeRequestStatus(int requestID, string status);
        bool ApproveRequest(int requestID);
        bool DisapproveRequest(int requestID);
        int NumberOfPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage);
    }
}
