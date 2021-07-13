﻿using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public class MediaDAO: IMediaDAO
    {
        public IEnumerable<Media> GetMediasByTitle(string searchValue)
        {
            try
            {
                string status = "Enabled";
                var medias = new List<Media>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = @status And MATCH (title)  AGAINST (@searchValue in boolean mode) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{searchValue}*");
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                }) ;
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Media> GetMediasByTitle(string searchValue, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode) " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", searchValue);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int NumberOfMediasBySearching(string searchValue)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(mediaID) " +
                                "From media " +
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode) " +
                                "and (status = 'Enabled' or status = 'Disabled')";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", searchValue);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public IEnumerable<Media> GetMediaFilterCategory(string CategoryName, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int NumberOfMediasFilterCategory(string CategoryName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public Media GetMediaByID(string mediaID)
        {
            try
            {
                Media media = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where mediaID = @mediaID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                media = new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return media;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoyName, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoyName and M.status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@CategoyName", CategoyName);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int NumberOfMediasFilterCategory_Status(string CategoyName, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoyName and M.status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoyName", CategoyName);
                    command.Parameters.AddWithValue("@Status", Status);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public IEnumerable<Media> GetMediasByTitleFilterCategory(string SearchValue, string CategoryName, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in natural language mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID=C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", SearchValue);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int NumberOfMediasBySearchingFilterCategory(string SearchValue, string CategoryName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in natural language mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID=C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", SearchValue);
                    command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public IEnumerable<Media> GetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in natural language mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", SearchValue);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int NumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in natural language mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", SearchValue);
                    command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    command.Parameters.AddWithValue("@Status", Status);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public bool RequestDisableMedia(string mediaID)
        {
            var result = false;
            try
            {
                Media media = GetMediaByID(mediaID);
                if (media.Status.Equals("Disabled")) return false;
                media.MediaID = mediaID + "_preview";
                string title_preview = media.Title + "_preview";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO media (mediaID, status, title, bannerURL, language, description) " +
                        "VALUES (@mediaID_preview, 'Pending', @title, @bannerURL, @language, @description) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID_preview", media.MediaID);
                        command.Parameters.AddWithValue("@title", title_preview);
                        command.Parameters.AddWithValue("@bannerURL", media.BannerURL);
                        command.Parameters.AddWithValue("@language", media.Language);
                        command.Parameters.AddWithValue("@description", media.Description);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool ApproveChangeMedia(string ID)
        {
            var result = false;
            try
            {
                string SqlUpdate = null;
                string SqlDelete = null;
                string NewTitle = null;
                string mediaID = ID.Split('_')[0];
                Media media = new Media();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (ID.Split('_')[1].Trim().ToLower().Equals("preview")) { 
                        SqlUpdate = "Update media " +
                            "Set title = @title, filmType = @filmType, director = @director, cast = @cast, " +
                            "publishYear = @publishYear, duration = @duration, bannerURL = @bannerURL, language = @language, description = @description " +
                            "Where mediaID = @mediaID";
                        media = GetMediaByID(ID);
                        NewTitle = media.Title.Split('_')[0];
                    }
                    if (ID.Split('_')[1].Trim().ToLower().Equals("enabled"))
                    {
                        SqlUpdate = "Update media " +
                            "Set status = 'Enabled' " +
                            "Where mediaID = @mediaID";
                    } 
                    if (ID.Split('_')[1].Trim().ToLower().Equals("disabled"))
                    {
                        SqlUpdate = "Update media " +
                            "Set status = 'Disabled' " +
                            "Where mediaID = @mediaID";
                    }
                    SqlDelete = "Delete from media " +
                        "Where mediaID = @ID";
                    MySqlCommand command1 = new MySqlCommand(SqlUpdate, connection);
                    MySqlCommand command2 = new MySqlCommand(SqlDelete, connection);
                    command1.Parameters.AddWithValue("@mediaID", mediaID);
                    command2.Parameters.AddWithValue("@ID", ID);
                    if (ID.Split('_')[1].Trim().ToLower().Equals("preview"))
                    {
                        command1.Parameters.AddWithValue("@title", NewTitle);
                        command1.Parameters.AddWithValue("@filmType", media.FilmType);
                        command1.Parameters.AddWithValue("@director", media.Director);
                        command1.Parameters.AddWithValue("@cast", media.Cast);
                        command1.Parameters.AddWithValue("@publishYear", media.PublishYear);
                        command1.Parameters.AddWithValue("@duration", media.Duration);
                        command1.Parameters.AddWithValue("@bannerURL", media.BannerURL);
                        command1.Parameters.AddWithValue("@language", media.Language);
                        command1.Parameters.AddWithValue("@description", media.Description);
                    }
                    int rowEffects1 = command1.ExecuteNonQuery();
                    int rowEffects2 = command2.ExecuteNonQuery();
                    if (rowEffects1 > 0 && rowEffects2 > 0)
                    {
                        result = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool DisapproveChangeMedia(string ID)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string SqlDelete ="Delete from media " +
                            "Where mediaID = @ID";
                    MySqlCommand command = new MySqlCommand(SqlDelete, connection);
                    command.Parameters.AddWithValue("@ID", ID);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        result = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public IEnumerable<Media> GetAllMedia(int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = 'Enabled' or status = 'Disabled' " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int NumberOfMedias()
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(mediaID) " +
                                "From media " +
                                "Where status = 'Enabled' or status = 'Disabled' ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public IEnumerable<Media> GetAllMediaFilterStatus(string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = @status and (status = 'Enabled' or status = 'Disabled') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@status", Status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int NumberOfMediasFilterStatus(string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(mediaID) " +
                                "From media " +
                                "Where status = @status and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@status", Status);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public IEnumerable<Media> GetMediasByTitleFilterStatus(string searchValue, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode) and status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", searchValue);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int NumberOfMediasBySearchingFilterStatus(string searchValue, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(mediaID) " +
                                "From media " +
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode) and status = @Status " +
                                "and (status = 'Enabled' or status = 'Disabled') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", searchValue);
                    command.Parameters.AddWithValue("@Status", Status);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public bool RequestChangeMediaStatus(string ID, string newStatus)
        {
            var result = false;
            try
            {
                string mediaID = ID.Split('_')[0];
                Media media = GetMediaByID(mediaID);
                if (media.Status.Trim().Equals("Pending")) return false;
                string title_preview = media.Title + "_preview";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO media (mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description) " +
                        "VALUES (@mediaID_preview, 'Pending', @title, @filmType, @director, @cast, @publishYear, @duration, @bannerURL, @language, @description) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID_preview", ID);
                        command.Parameters.AddWithValue("@title", title_preview);
                        command.Parameters.AddWithValue("@filmType", media.FilmType);
                        command.Parameters.AddWithValue("@director", media.Director);
                        command.Parameters.AddWithValue("@cast", media.Cast);
                        command.Parameters.AddWithValue("@publishYear", media.PublishYear);
                        command.Parameters.AddWithValue("@duration", media.Duration);
                        command.Parameters.AddWithValue("@bannerURL", media.BannerURL);
                        command.Parameters.AddWithValue("@language", media.Language);
                        command.Parameters.AddWithValue("@description", media.Description);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("fail. This media is requesting to change status");
            }
            return result;
        }
        public Media GetMediaByChildID(string childID, string type)
        {
            try
            {
                Media media = null;
                string Sql = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (type.Trim().Equals("media"))
                        Sql = "Select M.mediaID, M.status, M.title, M.filmType, M.director, M.cast, M.publishYear, M.duration, M.bannerURL, M.language, M.description " +
                                "From media M " +
                                "Where M.mediaID = @childID";
                    if (type.Trim().Equals("season"))
                        Sql = "Select M.mediaID, M.status, M.title, M.filmType, M.director, M.cast, M.publishYear, M.duration, M.bannerURL, M.language, M.description " +
                                "From media M, season S " +
                                "Where M.mediaID = S.mediaID and S.seasonID = @childID";
                    else if (type.Trim().Equals("episode"))
                        Sql = "Select M.mediaID, M.status, M.title, M.filmType, M.director, M.cast, M.publishYear, M.duration, M.bannerURL, M.language, M.description " + "From media M, season S, episode E " +
                                "Where M.mediaID = S.mediaID and S.seasonID = E.seasonID and E.episodeID = @childID";
                    else if (type.Trim().Equals("subtitle"))
                        Sql = "Select M.mediaID, M.status, M.title, M.filmType, M.director, M.cast, M.publishYear, M.duration, M.bannerURL, M.language, M.description " + "From media M, season S, episode E, subtitle ST " +
                                "Where M.mediaID = S.mediaID and S.seasonID = E.seasonID and E.episodeID = ST.episodeID and ST.subtitleID = @childID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@childID", childID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                media = new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return media;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Media> GetNewestMedias(int limit )
        {
            try
            {
                string status = "Enabled";
                var medias = new List<Media>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = @status " +
                                "Order By mediaID desc " +
                                "Limit @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@limit", limit);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                medias.Add(new Media
                                {
                                    MediaID = reader.GetString(0),
                                    Status = reader.GetString(1),
                                    Title = reader.GetString(2),
                                    FilmType = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Director = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Cast = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    PublishYear = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                                    Duration = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BannerURL = reader.GetString(8),
                                    Language = reader.GetString(9),
                                    Description = reader.GetString(10)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return medias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string AddMedia(string Title, string FilmType, string Director, string Cast, int? PublishYear, string Duration, string BannerURL, string Language, string Description)
        {
            string result = null;
            try
            {
                MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                AddMediaExecute();
                void AddMediaExecute()
                {
                    command.CommandText = "createMedia"; 
                    command.Parameters.AddWithValue("@title_Input", Title);
                    command.Parameters.AddWithValue("@filmType_Input", FilmType);
                    command.Parameters.AddWithValue("@director_Input", Director);
                    command.Parameters.AddWithValue("@cast_Input", Cast);
                    command.Parameters.AddWithValue("@publishYear_Input", PublishYear);
                    command.Parameters.AddWithValue("@duration_Input", Duration);
                    command.Parameters.AddWithValue("@bannerURL_Input", BannerURL);
                    command.Parameters.AddWithValue("@language_Input", Language);
                    command.Parameters.AddWithValue("@description_Input", Description);
                    command.Parameters.Add("@mediaID_Output", MySqlDbType.String).Direction
                        = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    result = (string)command.Parameters["@mediaID_Output"].Value;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail. " + ex.Message);
            }
            return result;
        }

        public string UpdateMedia(Media media)
        {
            string mediaID = null;
            try
            {
                var connection = new MySqlConnection(DbUtil.ConnectionString);
                var command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.CommandText = "updateMedia";
                command.Parameters.AddWithValue("@mediaID_InOutput", media.MediaID);
                command.Parameters.AddWithValue("@title_Input", media.Title);
                command.Parameters.AddWithValue("@filmType_Input", media.FilmType);
                command.Parameters.AddWithValue("@director_Input", media.Director);
                command.Parameters.AddWithValue("@cast_Input", media.Cast);
                command.Parameters.AddWithValue("@publishYear_Input", media.PublishYear);
                command.Parameters.AddWithValue("@duration_Input", media.Duration);
                command.Parameters.AddWithValue("@bannerURL_Input", media.BannerURL);
                command.Parameters.AddWithValue("@language_Input", media.Language);
                command.Parameters.AddWithValue("@description_Input", media.Description);
                command.Parameters.Add("@mediaID_InOutput", MySqlDbType.String).Direction
                    = ParameterDirection.Output;
                command.ExecuteNonQuery();
                mediaID = (string)command.Parameters["@mediaID_InOutput"].Value;
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mediaID;
        }
    }
}
