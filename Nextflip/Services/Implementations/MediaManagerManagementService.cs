using Nextflip.Models.mediaEditRequest;
using Nextflip.Models.media;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class MediaManagerManagementService : IMediaManagerManagementService
    {
        private IMediaEditRequestDAO _mediaEditRequestDao;
        private IMediaDAO _mediaDao;
        public MediaManagerManagementService(IMediaEditRequestDAO mediaEditRequestDao, IMediaDAO mediaDao)
        {
            _mediaEditRequestDao = mediaEditRequestDao;
            _mediaDao = mediaDao;
        }
        public IEnumerable<MediaEditRequest> GetAllPendingMedias() => _mediaEditRequestDao.GetAllPendingMedias();
        
        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue, int RowsOnPage, int RequestPage) 
                        => _mediaEditRequestDao.GetPendingMediaByUserEmail(searchValue, RowsOnPage, RequestPage);
        public int NumberOfPendingMediasBySearching(string searchValue) => _mediaEditRequestDao.NumberOfPendingMediasBySearching(searchValue);
        
        public bool ChangeRequestStatus(int requestID, string status) => _mediaEditRequestDao.ChangeRequestStatus(requestID, status);
        public bool ApproveRequest(int requestID) => _mediaEditRequestDao.ApproveRequest(requestID);
        public bool DisappoveRequest(int requestID, string note) => _mediaEditRequestDao.DisapproveRequest(requestID, note);
        
        public int NumberOfPendingMedias() => _mediaEditRequestDao.NumberOfPendingMedias();
        public IEnumerable<MediaEditRequest> GetPendingMediasListAccordingRequest(int RowsOnPage, int RequestPage)
                    => _mediaEditRequestDao.GetPendingMediasListAccordingRequest(RowsOnPage, RequestPage);
                    
        public int NumberOfPendingMediasFilterStatus(string status)
                => _mediaEditRequestDao.NumberOfPendingMediasFilterStatus(status);
        public IEnumerable<MediaEditRequest> GetPendingMediasFilterStatus(string status, int RowsOnPage, int RequestPage)
            => _mediaEditRequestDao.GetPendingMediasFilterStatus(status, RowsOnPage, RequestPage);

        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmailFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage)
            => _mediaEditRequestDao.GetPendingMediaByUserEmailFilterStatus(searchValue, Status, RowsOnPage, RequestPage);
        public int NumberOfPendingMediasBySearchingFilterStatus(string searchValue, string Status)
            => _mediaEditRequestDao.NumberOfPendingMediasBySearchingFilterStatus(searchValue, Status);

        public bool ApproveChangeMedia(string mediaID) => _mediaDao.ApproveChangeMedia(mediaID);
        public bool DisapproveChangeMedia(string mediaID) => _mediaDao.DisapproveChangeMedia(mediaID);
        public IEnumerable<MediaEditRequest> GetMediaRequest(string status, string type, int RowsOnPage, int RequestPage)
            =>_mediaEditRequestDao.GetMediaRequest(status, type, RowsOnPage, RequestPage);
        public int NumberOfMediaRequest(string status, string type)
            => _mediaEditRequestDao.NumberOfMediaRequest(status, type);
        public IEnumerable<MediaEditRequest> SearchingMediaRequest(string searchValue, string status, string type, int RowsOnPage, int RequestPage)
            => _mediaEditRequestDao.SearchingMediaRequest(searchValue, status, type, RowsOnPage, RequestPage);
        public int NumberOfMediaRequestSearching(string searchValue, string status, string type)
            => _mediaEditRequestDao.NumberOfMediaRequestSearching(searchValue, status, type);
    }
}
