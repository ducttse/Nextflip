using Nextflip.Models.mediaEditRequest;
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
        public MediaManagerManagementService(IMediaEditRequestDAO mediaEditRequestDao) => _mediaEditRequestDao = mediaEditRequestDao;
        public IEnumerable<MediaEditRequest> GetAllPendingMedias() => _mediaEditRequestDao.GetAllPendingMedias();
        public IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue) => _mediaEditRequestDao.GetPendingMediaByUserEmail(searchValue);
        public bool ApproveRequest(int requestID) => _mediaEditRequestDao.ApproveRequest(requestID);
        public bool DisappoveRequest(int requestID) => _mediaEditRequestDao.DisapproveRequest(requestID);
    }
}
