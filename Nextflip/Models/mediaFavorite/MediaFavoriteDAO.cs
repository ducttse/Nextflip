using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaFavorite
{
    public class MediaFavoriteDAO : IMediaFavoriteDAO
    {
        public string ConnectionString { get; set; }

        public async Task<IList<string>> GetMediaIDs(string favoriteListID)
        {
            var mediaIDs = new List<string>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string Sql = $"Select mediaID From MediaCategory Where favoriteListID = {favoriteListID}";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
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
