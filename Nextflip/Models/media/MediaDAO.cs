using MySql.Data.MySqlClient;
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
                var medias = new List<Media>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where (MATCH (title)  AGAINST (@searchValue in boolean mode)) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchValue", $"{searchValue}*");
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
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode)";
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
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName ";
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
                var media = new Media();
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
                                "where M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoyName and M.status = @Status ";
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
                                "and M.mediaID = MC.mediaID and MC.categoryID=C.categoryID and C.name = @CategoryName";
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
                                "and M.mediaID = MC.mediaID and MC.categoryID = C.categoryID and C.name = @CategoryName and M.status = @Status ";
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

        public bool ApproveChangeMediaStatus(string mediaID)
        {
            var result = false;
            try
            {
                string newStatus = null;
                Media media = GetMediaByID(mediaID);
                if (media.Status.Equals("Disabled")) newStatus = "Enabled";
                if (media.Status.Equals("Enabled")) newStatus = "Disabled";
                string mediaID_preview = mediaID + "_preview";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql1 = "Update media " +
                            "Set status = @newStatus " +
                            "Where mediaID = @mediaID";
                    string Sql2 = "Delete from media " +
                            "Where mediaID = @mediaID_preview";
                    MySqlCommand command1 = new MySqlCommand(Sql1, connection);
                    MySqlCommand command2 = new MySqlCommand(Sql2, connection);
                    command1.Parameters.AddWithValue("@newStatus", newStatus);
                    command1.Parameters.AddWithValue("@mediaID", mediaID);
                    command2.Parameters.AddWithValue("@mediaID_preview", mediaID_preview);
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
        public bool DisapproveChangeMediaStatus(string mediaID)
        {
            var result = false;
            try
            {
                string mediaID_preview = mediaID + "_preview";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Delete from media " +
                            "Where mediaID = @mediaID_preview";
                    MySqlCommand command = new MySqlCommand(Sql, connection);
                    command.Parameters.AddWithValue("@mediaID_preview", mediaID_preview);
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
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
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
                                "From media";
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
                    string Sql = "Select mediaID,status, title, filmType, director, cast, publishYear, duration, bannerURL, language, description " +
                                "From media " +
                                "Where status = @status " +
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
                                "Where status = @status";
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
                                "Where MATCH (title)  AGAINST (@searchValue in natural language mode) and status = @Status";
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
    }
}
