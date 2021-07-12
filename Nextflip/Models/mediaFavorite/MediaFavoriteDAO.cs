using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaFavorite
{
    public class MediaFavoriteDAO : IMediaFavoriteDAO
    {
        
        public IList<string> GetMediaIDs(string favoriteListID)
        {
            try
            {
                var mediaIDs = new List<string>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID " +
                                "From mediaFavoriteList " +
                                "Where favoriteListID = @favoriteListID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@favoriteListID", favoriteListID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mediaIDs.Add(reader.GetString(0));
                            }
                        }
                    }
                    connection.Close();
                }
                return mediaIDs;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool IsMediaFavoriteExisted(MediaFavorite mediaFavorite)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID, favoriteListID " +
                                "From mediaFavoriteList " +
                                "Where mediaID = @mediaID And favoriteListID = @favoriteListID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaFavorite.MediaID);
                        command.Parameters.AddWithValue("@favoriteListID", mediaFavorite.FavoriteListID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddMediaToFavorite(MediaFavorite mediaFavorite)
        {

            try
            {
                bool isMediaExisted = IsMediaFavoriteExisted(mediaFavorite);
                if (isMediaExisted == false)
                {
                    using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                    {
                        connection.Open();
                        string Sql = "Insert into mediaFavoriteList( mediaID, favoriteListID) " +
                                    "Values( @mediaID , @favoriteListID) ";
                        using (var command = new MySqlCommand(Sql, connection))
                        {
                            command.Parameters.AddWithValue("@mediaID", mediaFavorite.MediaID);
                            command.Parameters.AddWithValue("@favoriteListID", mediaFavorite.FavoriteListID);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                else
                {
                    throw new Exception("This media is existed in Favorite List");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveMediaFromFavorite(MediaFavorite mediaFavorite)
        {
            try
            {
                bool isMediaExisted = IsMediaFavoriteExisted(mediaFavorite);
                if (isMediaExisted == true)
                {
                    using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                    {
                        connection.Open();
                        string Sql = "Delete From mediaFavoriteList " +
                                    "Where mediaID = @mediaID And favoriteListID = @favoriteListID";
                        using (var command = new MySqlCommand(Sql, connection))
                        {
                            command.Parameters.AddWithValue("@mediaID", mediaFavorite.MediaID);
                            command.Parameters.AddWithValue("@favoriteListID", mediaFavorite.FavoriteListID);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                else
                {
                    throw new Exception("This media does not exist in Favorite List");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
