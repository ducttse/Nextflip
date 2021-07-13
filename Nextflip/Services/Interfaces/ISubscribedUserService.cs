using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.media;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;
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
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID);
        Season GetSeasonByID(string seasonID);
        //episode
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
        //subtitle
        Subtitle GetSubtitleByID(string subtitleID);
        IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID);
        // favorite
        void AddMediaToFavoriteList(string userID, string mediaID);
        void RemoveMediaFromFavoriteList(string userID, string mediaID);
        bool IsFavoriteMedia(string userID, string mediaID);
        IEnumerable<Media> GetNewestMedias(int limit);
    }
}
