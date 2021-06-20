using Nextflip.Models.mediaEditRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nextflip.Services.Interfaces
{
    public interface IMediaManagerManagementService
    {
        IEnumerable<MediaEditRequest> GetAllPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfPendingMediasBySearching(string searchValue);
        bool ChangeRequestStatus(int requestID, string status);
        bool ApproveRequest(int requestID);
        bool DisappoveRequest(int requestID);
        int NumberOfPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage);
        bool AddMediaRequest(string userEmail, string mediaID, string note);
    }
}
