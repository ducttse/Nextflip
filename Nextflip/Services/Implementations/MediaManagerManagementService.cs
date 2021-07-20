using Nextflip.Models.media;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;
using Nextflip.Models.episode;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;

namespace Nextflip.Services.Implementations
{
    public class MediaManagerManagementService : IMediaManagerManagementService
    {
        private IMediaDAO _mediaDao;
        private IEpisodeDAO _episodeDAO;
        private ISeasonDAO _seasonDAO;
        private ISubtitleDAO _subtitleDAO;
        public MediaManagerManagementService( IMediaDAO mediaDao, IEpisodeDAO episodeDao, 
            ISeasonDAO seasonDao, ISubtitleDAO subtitleDao)
        {
            _mediaDao = mediaDao;
            _episodeDAO = episodeDao;
            _seasonDAO = seasonDao;
            _subtitleDAO = subtitleDao;
        }

        public bool ApproveChangeMedia(string mediaID) => _mediaDao.ApproveChangeMedia(mediaID);
        public bool DisapproveChangeMedia(string mediaID) => _mediaDao.DisapproveChangeMedia(mediaID);
       
        public Media GetMediaByID(string mediaID) => _mediaDao.GetMediaByID(mediaID);
        public bool ApproveChangeEpisode(string episodeID) => _episodeDAO.ApproveChangeEpisode(episodeID);
        public bool DisapproveChangeEpisode(string episodeID) => _episodeDAO.DisapproveChangeEpisode(episodeID);
        public bool ApproveChangeSeason(string seasonID) => _seasonDAO.ApproveChangeSeason(seasonID);
        public bool DisapproveChangeSeason(string seasonID) => _seasonDAO.DisapproveChangeSeason(seasonID);
        public Season GetSeasonByID(string seasonID) => _seasonDAO.GetSeasonByID(seasonID);
        public Episode GetEpisodeByID(string episodeID) => _episodeDAO.GetEpisodeByID(episodeID);
        public ViewEditorDashboard.PrototypeMediaForm GetDetailedMediaByMediaId(string mediaId) => _mediaDao.GetDetailedMedia(mediaId);
    }
}
