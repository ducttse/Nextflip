using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.media
{
    public class MediaDAO: IMediaDAO
    {
        public string ConnectionString { get; set; }

        public async Task<IEnumerable<MediaDTO>> GetMediasByTitle(string searchValue)
        {
            var medias = new List<MediaDTO>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string Sql = $"Select mediaID, title, bannerURL, language, description " +
                                $"From Media Where title LIKE %{searchValue}%";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            medias.Add(new MediaDTO
                            {
                                MediaID = reader.GetString(0),
                                Title = reader.GetString(1),
                                BannerURL = reader.GetString(2),
                                Language = reader.GetString(3),
                                Description = reader.GetString(4),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return medias;
        }

        public async Task<MediaDTO> GetMediasByID(string mediaID)
        {
            var media = new MediaDTO();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string Sql = $"Select mediaID, status, title, bannerURL, language, description " +
                        $"From Media Where mediaID = {mediaID}";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            media = new MediaDTO
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
