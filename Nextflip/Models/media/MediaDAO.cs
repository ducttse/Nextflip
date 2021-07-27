using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.APIControllers;
using Nextflip.Models.episode;
using Nextflip.Models.mediaCategory;
using Nextflip.Models.season;

namespace Nextflip.Models.media
{
    public class MediaDAO: IMediaDAO
    {
        public IEnumerable<Media> GetMediasByTitle(string searchValue)
        {
            try
            {
                string status = "Approved";
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
                                "Where MATCH (title)  AGAINST (@searchValue in boolean mode) " +
                                "and (status = 'Approved' or status = 'Disapproved') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{searchValue}*");
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
                                "Where MATCH (title)  AGAINST (@searchValue in boolean mode) " +
                                "and (status = 'Approved' or status = 'Disapproved')";
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
                                "and (status = 'Approved' or status = 'Disapproved') " +
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
                                "and (status = 'Approved' or status = 'Disapproved') ";
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
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media " +
                                "Where mediaID = @mediaID and status != 'removed'";
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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

        public IEnumerable<Media> ViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql;
                    if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "where M.status = @Status " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
        public int NumberOfMediasFilterCategory_Status(string CategoryName, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql;
                if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M";
                }
                else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                        "From media M " +
                        "where M.status = @Status ";
                }
                else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M, mediaCategory MC, category C " +
                          "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName";
                }
                else
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M, mediaCategory MC, category C " +
                          "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID=C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Approved' or status = 'Disapproved') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID=C.categoryID and C.name = @CategoryName " +
                                "and (status = 'Approved' or status = 'Disapproved') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
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
                    string Sql;
                    if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.status = @Status " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
                        if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                string Sql;
                if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) ";
                }
                else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.status = @Status ";
                }
                else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName";
                }
                else
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
                    if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                if (media.Status.Equals("Disapproved")) return false;
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

        public void RemoveMedia(string mediaID)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {

                    using MySqlCommand command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "removeMedia";
                    command.Parameters.AddWithValue("@mediaID", mediaID);
                    int result = command.ExecuteNonQuery();
                    if (result == 0)
                    {
                        throw new Exception("Remove clone failed");
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Fail. " + exception.Message);
            }
        }

        public bool ApproveChangeMedia(string ID)
        {
            var result = false;
            try
            {
                var mediaForm = GetDetailedMedia(ID);
                if (mediaForm.MediaInfo.Title != null && mediaForm.MediaInfo.Title.EndsWith("_preview"))
                {
                    var trueTitle = mediaForm.MediaInfo.Title.Remove(mediaForm.MediaInfo.Title.LastIndexOfAny("_preview".ToCharArray()), 8);
                    var oldMediaID = GetMediasByTitle(trueTitle);
                    if (oldMediaID != null)
                    {
                        mediaForm.MediaInfo.MediaID = oldMediaID.FirstOrDefault().MediaID;
                        mediaForm.MediaInfo.Title = trueTitle;
                        EditMedia(mediaForm);
                        ApproveChangeMedia(mediaForm.MediaInfo.MediaID);
                        RemoveMedia(ID);
                    }
                }
                else
                {
                    using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                    {
                        connection.Open();
                        string sql = "approveMedia";
                        using (var command = new MySqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("mediaID_Input", ID);
                            int affectedRow = command.ExecuteNonQuery();
                            if (affectedRow == 1) result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool DisapproveChangeMedia(string ID, string note)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "disapproveMedia";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("mediaID_Input", ID);
                        command.Parameters.AddWithValue("note_Input", note);
                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow == 1) result = true;
                    }
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
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media " +
                                "Where status = 'Approved' or status = 'Disapproved' " +
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                                "Where status = 'Approved' or status = 'Disapproved' ";
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
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media " +
                                "Where status = @status and (status = 'Approved' or status = 'Disapproved') " +
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                                "Where status = @status and (status = 'Approved' or status = 'Disapproved') ";
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
                    string Sql = "Select mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media " +
                                "Where MATCH (title)  AGAINST (@searchValue in boolean mode) and status = @Status " +
                                "and (status = 'Approved' or status = 'Disapproved') " +
                                "ORDER BY status DESC " +
                                "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{searchValue}*");
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
                                "Where MATCH (title)  AGAINST (@searchValue in boolean mode) and status = @Status " +
                                "and (status = 'Approved' or status = 'Disapproved') ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"{searchValue}*");
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

        public bool RequestChangeMediaStatus(string ID, string newStatus, string note)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE media " +
                        "SET status = @newStatus, note = @note " +
                        "where mediaID = @ID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@newStatus", newStatus);
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@note", note);
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
                throw new Exception(ex.Message);
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
                string status = "Approved";
                var medias = new List<Media>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = @status " +
                                "Order By uploadDate desc " +
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
                command.Parameters.AddWithValue("@mediaID_InOutput", media.MediaID).Direction
                    = ParameterDirection.InputOutput; 
                command.Parameters.AddWithValue("@title_Input", media.Title);
                command.Parameters.AddWithValue("@filmType_Input", media.FilmType);
                command.Parameters.AddWithValue("@director_Input", media.Director);
                command.Parameters.AddWithValue("@cast_Input", media.Cast);
                command.Parameters.AddWithValue("@publishYear_Input", media.PublishYear);
                command.Parameters.AddWithValue("@duration_Input", media.Duration);
                command.Parameters.AddWithValue("@bannerURL_Input", media.BannerURL);
                command.Parameters.AddWithValue("@language_Input", media.Language);
                command.Parameters.AddWithValue("@description_Input", media.Description);
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

        public string AddMedia(Media mediaInfo)
        {
            string result = null;
            try
            {
                using MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
                using MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                AddMediaExecute();
                void AddMediaExecute()
                {
                    command.CommandText = "createMedia";
                    command.Parameters.AddWithValue("@title_Input", mediaInfo.Title);
                    command.Parameters.AddWithValue("@filmType_Input", mediaInfo.FilmType);
                    command.Parameters.AddWithValue("@director_Input", mediaInfo.Director);
                    command.Parameters.AddWithValue("@cast_Input", mediaInfo.Cast);
                    command.Parameters.AddWithValue("@publishYear_Input", mediaInfo.PublishYear);
                    command.Parameters.AddWithValue("@duration_Input", mediaInfo.Duration);
                    command.Parameters.AddWithValue("@bannerURL_Input", mediaInfo.BannerURL);
                    command.Parameters.AddWithValue("@language_Input", mediaInfo.Language);
                    command.Parameters.AddWithValue("@description_Input", mediaInfo.Description);
                    command.Parameters.Add("@mediaID_Output", MySqlDbType.String).Direction
                        = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    result = (string) command.Parameters["@mediaID_Output"].Value;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Fail. " + exception.Message);
            }
            return result;
        }

        public string AddNewMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm)
        {
            string newMediaID = null;
            using MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                newMediaID = AddMedia_Transact(connection, mediaForm.MediaInfo);
                if (newMediaID != null)
                {
                    mediaForm.MediaInfo.MediaID = newMediaID;
                    new MediaCategoryDAO().AddCategories_Transact(connection, mediaForm.MediaInfo.MediaID, mediaForm.CategoryIDs);
                    foreach (ViewEditorDashboard.PrototypeSeasonForm seasonForm in mediaForm.Seasons)
                    {
                        seasonForm.SeasonInfo.MediaID = newMediaID;
                        string newSeasonID = new SeasonDAO().AddSeason_Transact(connection, seasonForm.SeasonInfo);
                        if (newSeasonID != null)
                        {
                            foreach (Episode episode in seasonForm.Episodes)
                            {
                                episode.SeasonID = newSeasonID;
                                string newEpisodeID = new EpisodeDAO().AddEpisode_Transact(connection, episode);
                                episode.EpisodeID = newEpisodeID;
                            }
                        }
                    }
                    transaction.Commit();
                }
                else
                {
                    throw new Exception("Null media ID found");
                }
            }
            catch (Exception exception)
            {
                //try rollback
                try
                {
                    transaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    throw new Exception("Add new media failed, attempt to rollback also failed " + rollbackException.Message);
                }
                throw new Exception("Add new media failed " + exception.Message);

            }
            return newMediaID;
        }

        public ViewEditorDashboard.PrototypeMediaForm GetDetailedMedia(string mediaId)
        {
            ViewEditorDashboard.PrototypeMediaForm media = new ViewEditorDashboard.PrototypeMediaForm();
            media.MediaInfo = GetMediaByID(mediaId);
            media.CategoryIDs = new MediaCategoryDAO().GetCategoryIDs(mediaId);
            var seasonForms = new List<ViewEditorDashboard.PrototypeSeasonForm>();
            IEnumerable<Season> seasons = new SeasonDAO().GetSeasonsByMediaID(mediaId);
            //construct prototype season
            foreach(var season in seasons)
            {
                IEnumerable<Episode> episodes = new EpisodeDAO().GetEpisodesBySeasonID(season.SeasonID).OrderBy(episode => episode.Number);
                var prototypeSeason = new ViewEditorDashboard.PrototypeSeasonForm(season, episodes);
                seasonForms.Add(prototypeSeason);
            }
            media.Seasons = seasonForms.OrderBy(pSeason => pSeason.SeasonInfo.Number).ToList();
            return media;
        }

        private string AddMedia_Transact(MySqlConnection connection, Media mediaInfo)
        {
            string result = null;
            using MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            AddMediaExecute(); 
            void AddMediaExecute()
            {
                command.CommandText = "createMedia";
                command.Parameters.AddWithValue("@title_Input", mediaInfo.Title);
                command.Parameters.AddWithValue("@filmType_Input", mediaInfo.FilmType);
                command.Parameters.AddWithValue("@director_Input", mediaInfo.Director);
                command.Parameters.AddWithValue("@cast_Input", mediaInfo.Cast);
                command.Parameters.AddWithValue("@publishYear_Input", mediaInfo.PublishYear);
                command.Parameters.AddWithValue("@duration_Input", mediaInfo.Duration);
                command.Parameters.AddWithValue("@bannerURL_Input", mediaInfo.BannerURL);
                command.Parameters.AddWithValue("@language_Input", mediaInfo.Language);
                command.Parameters.AddWithValue("@description_Input", mediaInfo.Description);
                command.Parameters.AddWithValue("@status_Input", mediaInfo.Status);
                command.Parameters.Add("@mediaID_Output", MySqlDbType.String).Direction
                    = ParameterDirection.Output;
                command.ExecuteNonQuery();
                result = (string)command.Parameters["@mediaID_Output"].Value;
            }
            return result;
        }
        public string CloneMedia(string mediaID)
        {
            string cloneMediaID = null;
            using MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
            connection.Open();
            try
            {
                string title = GetMediaByID(mediaID).Title + "_preview";
                using var command = connection.CreateCommand();
                command.CommandText = "select mediaID " +
                                      "from media " +
                                      "where status != 'removed' and title = @title";
                command.Parameters.AddWithValue("@title", title);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read() && reader.IsDBNull(0) is false)
                    {
                        //clone media already existed in database
                        cloneMediaID = reader.GetString(0);
                    }
                }
                //clone media not exist
                if (cloneMediaID is null)
                {
                    ViewEditorDashboard.PrototypeMediaForm detailCloneMedia = GetDetailedMedia(mediaID);
                    detailCloneMedia.MediaInfo.Title += "_preview";
                    detailCloneMedia.MediaInfo.Status = "Ready";
                    cloneMediaID = AddNewMedia(detailCloneMedia);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Fail. " + exception.Message);
            }
            return cloneMediaID;

        }

        public string EditMedia(ViewEditorDashboard.PrototypeMediaForm mediaForm)
        {
            string mediaID = null;
            var oldMedia = GetDetailedMedia(mediaForm.MediaInfo.MediaID);
            using MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                ///try to update when media information changed
                if (!oldMedia.MediaInfo.EqualWithoutStatus(mediaForm.MediaInfo))
                {
                    int result = UpdateMedia_Transact(connection, mediaForm.MediaInfo);
                    if (result == 0)
                    {
                        throw new Exception("Update media information failed");
                    }
                }
                //re-add categories
                new MediaCategoryDAO().AddCategories_Transact(connection, mediaForm.MediaInfo.MediaID, mediaForm.CategoryIDs);
                ///remove all old information
                foreach (var oldSeasonForm in oldMedia.Seasons)
                {
                    foreach (var episode in oldSeasonForm.Episodes)
                    {
                        int episodeResult = new EpisodeDAO().RemoveEpisode_Transact(connection, episode.EpisodeID);
                    }
                    int seasonResult = new SeasonDAO().RemoveSeason_Transact(connection, oldSeasonForm.SeasonInfo.SeasonID);
                }
                ///apply change to dtb
                foreach (var newSeasonForm in mediaForm.Seasons)
                {
                    var oldSeason =
                        oldMedia.Seasons.FirstOrDefault(ss => ss.SeasonInfo.SeasonID == newSeasonForm.SeasonInfo.SeasonID);
                    ///check if not changed
                    if (newSeasonForm.SeasonInfo.EqualWithoutStatus(oldSeason.SeasonInfo)) {
                        if (newSeasonForm.Episodes.Count() == oldSeason.Episodes.Count())
                        {
                            bool flag = false;
                            var list1 = newSeasonForm.Episodes.ToArray();
                            var list2 = oldSeason.Episodes.ToArray();
                            for (int i = 0; i < list1.Length; ++i)
                            {
                                if (!list1[i].EqualWithoutStatus(list2[i]))
                                {
                                    flag = true;
                                }
                            }
                            if (flag == false)
                            {
                                newSeasonForm.SeasonInfo.Status = oldSeason.SeasonInfo.Status;
                            }
                        }
                    }

                    string newSeasonID = new SeasonDAO().AddSeason_Transact(connection, newSeasonForm.SeasonInfo);
                    foreach (var episode in newSeasonForm.Episodes)
                    {
                        if (episode.EpisodeID != null && episode.EpisodeID.Trim() != "" && oldSeason != null)
                        {
                            var oldEpisode = oldSeason.Episodes.Where(ep => ep.EpisodeID == episode.EpisodeID).FirstOrDefault();
                            if (episode.EqualWithoutStatus(oldEpisode))
                            {
                                episode.Status = oldEpisode.Status;
                            }
                        }
                        episode.SeasonID = newSeasonID;
                        string episodeID = new EpisodeDAO().AddEpisode_Transact(connection, episode);
                        episode.EpisodeID = episodeID;
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                //try rollback
                try
                {
                    transaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    throw new Exception("Edit media failed, attempt to rollback also failed " + rollbackException.Message);
                }
                throw new Exception("Edit media failed " + exception.Message);
            }
            return mediaID;
        }

        private int UpdateMedia_Transact(MySqlConnection connection, Media mediaInfo)
        {
            int result = 0;
            using MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            AddMediaExecute();
            void AddMediaExecute()
            {
                command.CommandText = "updateMedia";
                command.Parameters.AddWithValue("@mediaID_Input", mediaInfo.MediaID);
                command.Parameters.AddWithValue("@title_Input", mediaInfo.Title);
                command.Parameters.AddWithValue("@filmType_Input", mediaInfo.FilmType);
                command.Parameters.AddWithValue("@director_Input", mediaInfo.Director);
                command.Parameters.AddWithValue("@cast_Input", mediaInfo.Cast);
                command.Parameters.AddWithValue("@publishYear_Input", mediaInfo.PublishYear);
                command.Parameters.AddWithValue("@duration_Input", mediaInfo.Duration);
                command.Parameters.AddWithValue("@bannerURL_Input", mediaInfo.BannerURL);
                command.Parameters.AddWithValue("@language_Input", mediaInfo.Language);
                command.Parameters.AddWithValue("@description_Input", mediaInfo.Description);
                result = command.ExecuteNonQuery();
            }
            return result;
        }

        public IEnumerable<Media> EditorViewMediasFilterCategory_Status(string CategoryName, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql;
                    if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID, status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "where M.status = @Status " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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

        public int EditorNumberOfMediasFilterCategory_Status(string CategoryName, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql;
                if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M ";
                }
                else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                        "From media M " +
                        "where M.status = @Status";
                }
                else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M, mediaCategory MC, category C " +
                          "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName ";
                }
                else
                {
                    Sql = "Select COUNT(M.mediaID) " +
                          "From media M, mediaCategory MC, category C " +
                          "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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

        public IEnumerable<Media> EditorGetMediasByTitleFilterCategory_Status(string SearchValue, string CategoryName, string Status, int RowsOnPage, int RequestPage)
        {
            try
            {
                var medias = new List<Media>();
                int offset = ((int)(RequestPage - 1)) * RowsOnPage;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql;
                    if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.status = @Status " +
                                "Order by uploadDate ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    else
                    {
                        Sql = "Select M.mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description, uploadDate, note " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status " +
                                "Order by status ASC " +
                                "LIMIT @offset, @limit";
                    }
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
                        if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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
                                    Description = reader.GetString(10),
                                    UploadDate = reader.GetDateTime(11),
                                    Note = reader.IsDBNull(12) ? null : reader.GetString(12)
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
        public int EditorNumberOfMediasBySearchingFilterCategory_Status(string SearchValue, string CategoryName, string Status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql;
                if (CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode)";
                }
                else if (CategoryName.Trim().ToLower().Equals("all") && !Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.status = @Status";
                }
                else if (!CategoryName.Trim().ToLower().Equals("all") && Status.Trim().ToLower().Equals("all"))
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName";
                }
                else
                {
                    Sql = "Select COUNT(M.mediaID) " +
                                "From media M, mediaCategory MC, category C " +
                                "Where MATCH (M.title)  AGAINST (@searchValue in boolean mode) " +
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status ";
                }
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"{SearchValue}*");
                    if (!CategoryName.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    if (!Status.Trim().ToLower().Equals("all")) command.Parameters.AddWithValue("@Status", Status);
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

    }
}
