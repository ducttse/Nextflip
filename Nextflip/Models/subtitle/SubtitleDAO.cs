using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models;
using MySql.Data.MySqlClient;
using Nextflip.utils;

namespace Nextflip.Models.subtitle
{
    public class SubtitleDAO : ISubtitleDAO
    {
        
        public SubtitleDTO GetSubtitleByEpisodeID(string episodeID)
        {
            SubtitleDTO subtitle = null;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = $"Select subtitleID, language, status, subtitleURL From subtitle Where episodeID = '{episodeID}'";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subtitle = new SubtitleDTO
                            {
                                SubtitleID = reader.GetString(0),
                                EpisodeID = episodeID,
                                Language = reader.GetString(1),
                                Status = reader.GetString(2),
                                SubtitleURL = reader.GetString(3)
                            };
                        }
                    }
                }
                connection.Close();
            }
            return subtitle;
        }
    }
}
