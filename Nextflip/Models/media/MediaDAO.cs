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
            var medias = new List<Media>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select mediaID,status, title, bannerURL, language, description " +
                                "From media Where title LIKE @searchValue";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
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

        public Media GetMediaByID(string mediaID)
        {
            var media = new Media();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql =    "Select mediaID, status, title, bannerURL, language, description " +
                                "From media Where mediaID = @mediaID";
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
                                BannerURL = reader.GetString(3),
                                Language = reader.GetString(4),
                                Description = reader.GetString(5),
                            };
                        }
                    }
                }
                connection.Close();
            }
            return media;
        }
    }
}
