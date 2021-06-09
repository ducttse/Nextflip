using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.season
{
    public class SeasonDAO: ISeasonDAO
    {
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID)
        {
            var seasons = new List<Season>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select seasonID, title, thumbnailURL, status, number From season Where mediaID = @mediaID";

                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@mediaID", mediaID);
                    using (var reader = command.ExecuteReader())
                    { 
                        while (reader.Read())
                        {
                            seasons.Add(new Season
                            {
                                SeasonID = reader.GetString(0),
                                Title = reader.GetString(1),
                                ThumbnailURL = reader.GetString(2) ,
                                MediaID = mediaID,
                                Status = reader.GetString(3),
                                Number = reader.GetInt32(4)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return seasons;
        }
    }
}
