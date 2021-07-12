using Nextflip.Models.mediaEditRequest;
using Nextflip.Models.media;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.episode;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;

namespace Nextflip.Services.Implementations
{
    public class MediaManagerManagementService : IMediaManagerManagementService
    {
        private IMediaEditRequestDAO _mediaEditRequestDao;
        private IMediaDAO _mediaDao;
        private IEpisodeDAO _episodeDAO;
        private ISeasonDAO _seasonDAO;
        private ISubtitleDAO _subtitleDAO;
        public MediaManagerManagementService(IMediaEditRequestDAO mediaEditRequestDao, IMediaDAO mediaDao, IEpisodeDAO episodeDao, 
            ISeasonDAO seasonDao, ISubtitleDAO subtitleDao)
        {
            _mediaEditRequestDao = mediaEditRequestDao;
            _mediaDao = mediaDao;
            _episodeDAO = episodeDao;
            _seasonDAO = seasonDao;
            _subtitleDAO = subtitleDao;
        }
        public IEnumerable<MediaEditRequest> GetAllPendingMedias() => _mediaEditRequestDao.GetAllPendingMedias();
        
        public IEnumerable<MediaEditRequest> GetRequestMediaFilterStatus(string userEmail, string Status, int RowsOnPage, int RequestPage)
                        => _mediaEditRequestDao.GetRequestMediaFilterStatus(userEmail, Status, RowsOnPage, RequestPage);
        public int NumberOfRequestMediaFilterStatus(string userEmail, string Status) => _mediaEditRequestDao.NumberOfRequestMediaFilterStatus(userEmail, Status);
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
        public Media GetMediaByID(string mediaID) => _mediaDao.GetMediaByID(mediaID);
        public MediaEditRequest GetMediaEditRequestByID(int requestID) => _mediaEditRequestDao.GetMediaEditRequestByID(requestID);
        public bool ApproveChangeEpisode(string episodeID) => _episodeDAO.ApproveChangeEpisode(episodeID);
        public bool DisapproveChangeEpisode(string episodeID) => _episodeDAO.DisapproveChangeEpisode(episodeID);
        public bool ApproveChangeSeason(string seasonID) => _seasonDAO.ApproveChangeSeason(seasonID);
        public bool DisapproveChangeSeason(string seasonID) => _seasonDAO.DisapproveChangeSeason(seasonID);
        public bool ApproveChangeSubtitle(string subtitleID) => _subtitleDAO.ApproveChangeSubtitle(subtitleID);
        public bool DisapproveChangeSubtitle(string subtitleID) => _subtitleDAO.DisapproveChangeSubtitle(subtitleID);
        public Season GetSeasonByID(string seasonID) => _seasonDAO.GetSeasonByID(seasonID);
        public Episode GetEpisodeByID(string episodeID) => _episodeDAO.GetEpisodeByID(episodeID);

    }
}
