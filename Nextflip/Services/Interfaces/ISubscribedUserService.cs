﻿using Nextflip.Models.category;
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
        IEnumerable<Media> GetFavoriteMediasByUserID(string userID);
        IEnumerable<Media> GetMediasByTitle(string title);
        IEnumerable<Media> GetMediasByCategoryID(int categoryID);
        //season
        IEnumerable<Season> GetSeasonsByMediaID(string mediaID);
        Season GetSeasonByID(string seasonID);
        //episode
        IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID);
        Episode GetEpisodeByID(string episodeID);
        //subtitle
        Subtitle GetSubtitleByID(string subtitleID);
        IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID);
        // datlt 
        IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearching(string searchValue);

        IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName);

        IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status);

        IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory(string CategoryName);
        IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage);
        int NumberOfMediasFilterCategory_Status(string CategoryName, string Status);
        bool ChangeMediaStatus(string mediaID, string status);
    }
}
