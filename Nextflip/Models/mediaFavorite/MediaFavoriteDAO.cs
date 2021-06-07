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
            var mediaIDs = new List<string>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = $"Select mediaID From mediaCategory Where favoriteListID = '{favoriteListID}'";
                using (var command = new MySqlCommand(Sql, connection))
                { 
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

        
    }
}
