using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaEditRequest
{
    public interface IMediaEditRequestDAO
    {
        IEnumerable<MediaEditRequest> GetAllPendingMedias();
        IEnumerable<MediaEditRequest> GetRequestMediaFilterStatus(string userEmail, string Status, int RowsOnPage, int RequestPage);
        int NumberOfRequestMediaFilterStatus(string userEmail, string Status);
        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmailFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage);
        int NumberOfPendingMediasBySearchingFilterStatus(string searchValue, string Status);

        bool ChangeRequestStatus(int requestID, string status);
        bool ApproveRequest(int requestID);
        bool DisapproveRequest(int requestID, string note);

        int NumberOfPendingMedias();
        IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage);


        int NumberOfPendingMediasFilterStatus(string status);
        IEnumerable<MediaEditRequest> GetPendingMediasFilterStatus(string status, int RowsOnPage, int RequestPage);

        bool AddMediaRequest(string userEmail, string mediaID, string note, string type, string ID);
        MediaEditRequest GetMediaEditRequestByID(int requestID);
        IEnumerable<MediaEditRequest> GetMediaRequest(string status, string type, string sortBy, int RowsOnPage, int RequestPage);
        int NumberOfMediaRequest(string status, string type);
        IEnumerable<MediaEditRequest> SearchingMediaRequest(string searchValue, string status, string sortBy, string type, int RowsOnPage, int RequestPage);
        int NumberOfMediaRequestSearching(string searchValue, string status, string type);
        IEnumerable<MediaEditRequest> SearchingRequestMediaFilterStatus(string searchValue, string userEmail, string Status, int RowsOnPage, int RequestPage);
        int NumberOfSearchingRequestMediaFilterStatus(string searchValue, string userEmail, string Status);
    }
}
