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
        IEnumerable<MediaEditRequest> GetPendingMediaByUserEmail(string searchValue);
        bool ApproveRequest(int requestID);
        bool DisappoveRequest(int requestID);
    }
}
