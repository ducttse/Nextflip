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
        public IEnumerable<Media> GetFavoriteMediasByUserID(string userID, int limit , int page)
        {
            var favoriteMedias = new List<Media>();
            try
            {
                FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
                if (favoriteList != null)
                {
                    IList<string> favoriteMediaIDs = _mediaFavoriteDAO.GetMediaIDs(favoriteList.FavoriteListID);
                    int end = limit * (page + 1);
                    end = favoriteMediaIDs.Count > end ? end : favoriteMediaIDs.Count;
                    int start = limit * (page - 1);
                    for (int i = start; i < end; i++)
                    {
                        Media media = _mediaDAO.GetMediaByID(favoriteMediaIDs[i]);
                        if (media.Status.Equals("Enabled"))
                        {
                            favoriteMedias.Add(media);
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return favoriteMedias;
        }
   //     public IEnumerable<Media> GetMediasByTitle(string title) => _mediaDAO.GetMediasByTitle(title);
        public IEnumerable<Media> GetMediasByCategoryID(int categoryID, int limit )
        {
            var medias = new List<Media>();
            IList<string> mediaIDs = _mediaCategoryDAO.GetMediaIDs(categoryID,limit);
            foreach (var mediaID in mediaIDs)
            { 
                Media media = _mediaDAO.GetMediaByID(mediaID);
                medias.Add(media);
            }
            return medias;
        }
        //season
        public Season GetSeasonByID(string seasonID) => _seasonDAO.GetSeasonByID(seasonID);
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID,string status ) => _seasonDAO.GetSeasonsByMediaID(mediaID,status);
        //episode
        public Episode GetEpisodeByID(string episodeID) => _episodeDAO.GetEpisodeByID(episodeID);

        IEnumerable<Episode> ISubscribedUserService.GetEpisodesBySeasonID(string seasonID,string status)
                                            => _episodeDAO.GetEpisodesBySeasonID(seasonID, status);
        //subtitle
        public Subtitle GetSubtitleByID(string subtitleID) => _subtitleDAO.GetSubtitleByID(subtitleID);
        public IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID) => _subtitleDAO.GetSubtitlesByEpisodeID(episodeID);
        // favorite list
        public void AddMediaToFavoriteList(string userID, string mediaID)
        {
            try
            {
                FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
                if (favoriteList == null) _favoriteListDAO.AddNewFavoriteList(userID);
                favoriteList = _favoriteListDAO.GetFavoriteList(userID);
                MediaFavorite mediaFavorite = new MediaFavorite { MediaID = mediaID, FavoriteListID = favoriteList.FavoriteListID };
                _mediaFavoriteDAO.AddMediaToFavorite(mediaFavorite);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RemoveMediaFromFavoriteList(string userID, string mediaID) 
        {
            try
            {
                FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
                if (favoriteList != null)
                {
                    MediaFavorite mediaFavorite = new MediaFavorite { MediaID = mediaID, FavoriteListID = favoriteList.FavoriteListID };
                    _mediaFavoriteDAO.RemoveMediaFromFavorite(mediaFavorite);
                }
                else
                {
                    throw new Exception("Favorite List does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsFavoriteMedia(string userID, string mediaID)
        {
            try
            {
                FavoriteList favoriteList = _favoriteListDAO.GetFavoriteList(userID);
                if (favoriteList != null)
                {
                    MediaFavorite mediaFavorite = new MediaFavorite
                    {
                        FavoriteListID = favoriteList.FavoriteListID,
                        MediaID = mediaID
                    };
                    bool isFavoriteMedia = _mediaFavoriteDAO.IsMediaFavoriteExisted(mediaFavorite);
                    return isFavoriteMedia;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Media> GetNewestMedias(int limit) => _mediaDAO.GetNewestMedias(limit);
    }
}

