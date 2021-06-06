using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Nextflip.Models.favoriteList
{
    public class FavoriteListDAO : IFavoriteListDAO
    {
        public string ConnectionString { get; set; }

        public async Task<FavoriteListDTO> GetFavoriteList(string userID)
        {
            FavoriteListDTO favoriteList = null;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string Sql = $"Select favoriteListID From favoriteList Where userID = {userID}";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            favoriteList = new FavoriteListDTO
                            {
                                FavoriteListID = reader.GetString(0),
                                UserID = userID,
                            };
                        }
                    }
                }
                connection.Close();
            }
            return favoriteList;
        }
    }
}
