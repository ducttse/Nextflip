using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISubscribedUserService
    {
        //category
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategoriesByMediaID(string mediaID);
        //media
        Media GetMediaByID(string mediaID);
        IEnumerable<Media> GetFavoriteMediasByUserID(string userID, int limit, int page);
        IEnumerable<Media> GetMediasByTitle(string title);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID, int limit );
        //season
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID, string status);
        Season GetSeasonByID(string seasonID);
        //episode
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID, string status);
        Episode GetEpisodeByID(string episodeID);
        // favorite
        void AddMediaToFavoriteList(string userID, string mediaID);
        void RemoveMediaFromFavoriteList(string userID, string mediaID);
        bool IsFavoriteMedia(string userID, string mediaID);
        IEnumerable<Media> GetNewestMedias(int limit);
    }
}
