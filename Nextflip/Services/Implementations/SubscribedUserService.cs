using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaCategory;
using Nextflip.Models.mediaFavorite;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class SubscribedUserService : ISubscribedUserService
    {
        private readonly ICategoryDAO _categoryDAO;
        private readonly IMediaCategoryDAO _mediaCategoryDAO;
        private readonly IFavoriteListDAO _favoriteListDAO;
        private readonly IMediaFavoriteDAO _mediaFavoriteDAO;
        private readonly IMediaDAO _mediaDAO;
        private readonly ISeasonDAO _seasonDAO;
        private readonly IEpisodeDAO _episodeDAO;
        private readonly ISubtitleDAO _subtitleDAO;
        public SubscribedUserService(ICategoryDAO categoryDAO, IMediaCategoryDAO mediaCategoryDAO,
                                IFavoriteListDAO favoriteListDAO, IMediaFavoriteDAO mediaFavoriteDAO,
                                IMediaDAO mediaDAO, ISeasonDAO seasonDAO, IEpisodeDAO episodeDAO, ISubtitleDAO subtitleDAO)
        {
            _categoryDAO = categoryDAO;
            _mediaCategoryDAO = mediaCategoryDAO;
            _favoriteListDAO = favoriteListDAO;
            _mediaFavoriteDAO = mediaFavoriteDAO;
            _mediaDAO = mediaDAO;
            _seasonDAO = seasonDAO;
            _episodeDAO = episodeDAO;
            _subtitleDAO = subtitleDAO;
        }

        //category
        public IEnumerable<Category> GetCategories() => _categoryDAO.GetCategories();

        public IEnumerable<Category> GetCategoriesByMediaID(string mediaID)
        {
            var categories = new List<Category>();
            IList<int> categoryIDs = _mediaCategoryDAO.GetCategoryIDs(mediaID);
            foreach (var categoryID in categoryIDs)
            {
                Category category = _categoryDAO.GetCategoryByID(categoryID);
                categories.Add(category);
            }
            return categories;
        }

        //media
        public Media GetMediaByID(string mediaID) => _mediaDAO.GetMediaByID(mediaID);

        public IEnumerable<Media> GetMediasByTitle(string searchValue) => _mediaDAO.GetMediasByTitle(searchValue);
        public IEnumerable<Media> GetFavoriteMediasByUserID(string userID)
        {
            var favoriteMedias = new List<Media>();
            FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
            IList<string> favoriteMediaIDs = _mediaFavoriteDAO.GetMediaIDs(favoriteList.FavoriteListID);
            foreach (string mediaID in favoriteMediaIDs)
            {
                favoriteMedias.Add(_mediaDAO.GetMediaByID(mediaID));
            }
            return favoriteMedias;
        }
   //     public IEnumerable<Media> GetMediasByTitle(string title) => _mediaDAO.GetMediasByTitle(title);
        public IEnumerable<Media> GetMediasByCategoryID(int categoryID, int limit = 20)
        {
            var medias = new List<Media>();
            IList<string> mediaIDs = _mediaCategoryDAO.GetMediaIDs(categoryID);
            for(int i = 0; i< limit; i++)
            {
                string mediaID = mediaIDs[i];
                Media media = _mediaDAO.GetMediaByID(mediaID);
                medias.Add(media);
            }
            return medias;
        }
        //season
        public Season GetSeasonByID(string seasonID) => _seasonDAO.GetSeasonByID(seasonID);
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID) => _seasonDAO.GetSeasonsByMediaID(mediaID);
        //episode
        public Episode GetEpisodeByID(string episodeID) => _episodeDAO.GetEpisodeByID(episodeID);

        IEnumerable<Episode> ISubscribedUserService.GetEpisodesBySeasonID(string seasonID)
                                            => _episodeDAO.GetEpisodesBySeasonID(seasonID);
        //subtitle
        public Subtitle GetSubtitleByID(string subtitleID) => _subtitleDAO.GetSubtitleByID(subtitleID);
        public IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID) => _subtitleDAO.GetSubtitlesByEpisodeID(episodeID);
        
    }
}

