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
                                "Where status = 'Enabled' And (MATCH (title)  AGAINST (@searchValue in boolean mode)) ";
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
                    string Sql = "Select mediaID,status, title, bannerURL, language, description " +
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
                                    BannerURL = reader.GetString(3),
                                    Language = reader.GetString(4),
                                    Description = reader.GetString(5),
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
                    string Sql = "Select M.mediaID, status, title, bannerURL, language, description " +
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
                                    BannerURL = reader.GetString(3),
                                    Language = reader.GetString(4),
                                    Description = reader.GetString(5),
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
                    string Sql = "Select M.mediaID, status, title, bannerURL, language, description " +
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
                                    BannerURL = reader.GetString(3),
                                    Language = reader.GetString(4),
                                    Description = reader.GetString(5),
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
                    string Sql = "Select M.mediaID, status, title, bannerURL, language, description " +
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
                                    BannerURL = reader.GetString(3),
                                    Language = reader.GetString(4),
                                    Description = reader.GetString(5),
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
                    string Sql = "Select M.mediaID, status, title, bannerURL, language, description " +
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
                                    BannerURL = reader.GetString(3),
                                    Language = reader.GetString(4),
                                    Description = reader.GetString(5),
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

        public bool ChangeMediaStatus(string mediaID, string status)
        {
            var result = false;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "UPDATE media " +
                    "SET status = @status " +
                    "WHERE mediaID = @mediaID";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@mediaID", mediaID);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        result = true;
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
