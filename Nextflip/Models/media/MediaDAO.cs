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
                MySqlCommand command1;
                MySqlCommand command2;
                int rowEffects1 = 0;
                int rowEffects2 = 0;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (mediaID.Equals(ID))
                    {
                        SqlUpdate = "Update media " +
                            "Set status = 'Enabled' " +
                            "Where mediaID = @mediaID";
                    } else {
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
                        command2 = new MySqlCommand(SqlDelete, connection);
                        command2.Parameters.AddWithValue("@ID", ID);
                        rowEffects2 = command2.ExecuteNonQuery();
                    }
                    command1 = new MySqlCommand(SqlUpdate, connection);
                    command1.Parameters.AddWithValue("@mediaID", mediaID);
                    if (!mediaID.Equals(ID)) {
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
                    }
                    rowEffects1 = command1.ExecuteNonQuery();
                    if (rowEffects1 > 0 && rowEffects2 > 0)
                    {
                        result = true;
                    }
                    if (rowEffects1 > 0 && mediaID.Equals(ID))
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
            int rowDeleteMDEffects = 0;
            string SqlMedia;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (ID.Split('_')[0].Equals(ID))
                    {
                        string SqlDeleteMediaCategory = "Delete from mediaCategory " +
                            "Where mediaID = @ID";
                        MySqlCommand commandDeleteMD = new MySqlCommand(SqlDeleteMediaCategory, connection);
                        commandDeleteMD.Parameters.AddWithValue("@ID", ID);
                        rowDeleteMDEffects = commandDeleteMD.ExecuteNonQuery();
                        SqlMedia = "Update media " +
                                "Set status = 'Disabled' " +
                                "Where mediaID = @ID";
                    } else
                    SqlMedia ="Delete from media " +
                            "Where mediaID = @ID";
                    MySqlCommand command = new MySqlCommand(SqlMedia, connection);
                    command.Parameters.AddWithValue("@ID", ID);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowDeleteMDEffects > 0 && rowEffects > 0 && ID.Split('_')[0].Equals(ID))
                    {
                        result = true;
                    }
                    if (rowEffects > 0 && !ID.Split('_')[0].Equals(ID))
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
                using var command = connection.CreateCommand();
                command.CommandText = "select mediaID into cloneMediaID_Output " +
                    "from media " +
                    "where status != 'removed' and concat(title, '_preview') in " +
                    "(select title from media where media.mediaID = @mediaID) ";
                command.Parameters.AddWithValue("@mediaID", mediaID);
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
                        oldMedia.Seasons.First(ss => ss.SeasonInfo.SeasonID == newSeasonForm.SeasonInfo.SeasonID);
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
                            var oldEpisode = oldSeason.Episodes.Where(ep => ep.EpisodeID == episode.EpisodeID).First();
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
    }
}
