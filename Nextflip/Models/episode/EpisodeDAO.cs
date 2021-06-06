using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public class EpisodeDAO : IEpisodeDAO
    {
        public string ConnectionString { get; set; }
        public IEnumerable<EpisodeDTO> GetEpisodesBySeasonID(string seasonID)
        {
            var episodes = new List<EpisodeDTO>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string Sql = $"Select episodeID, status, number From season Where seasonID = {seasonID}";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            episodes.Add(new EpisodeDTO
                            {
                                EpisodeID = reader.GetString(0),
                                SeasonID = seasonID,
                                Status = reader.GetString(1),
                                Number = reader.GetInt32(2)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return episodes;
        }
    }
}
