using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Nextflip.Models.favoriteList
{
    public class FavoriteListDAO : IFavoriteListDAO
    {
        
        public FavoriteList GetFavoriteList(string userID)
        {
            try
            {
                FavoriteList favoriteList = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select favoriteListID " +
                                "From favoriteList " +
                                "Where userID = @userID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                favoriteList = new FavoriteList
                                {
                                    FavoriteListID = reader.GetString(0),
                                    UserID = userID
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return favoriteList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddNewFavoriteList(string userID)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Insert into favoriteList( userID, favoriteListID) " +
                                "Values( @userID , @favoriteListID) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        command.Parameters.AddWithValue("@favoriteListID", generateID());
                        string generateID()
                        {
                            return Guid.NewGuid().ToString("N");
                        }
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
