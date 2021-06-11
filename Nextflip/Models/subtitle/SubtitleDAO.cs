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

        public Subtitle GetSubtitleByID(string subtitleID)
        {
            try
            {
                Subtitle subtitle = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select episodeID, language, status, subtitleURL " +
                                "From subtitle " +
                                "Where subtitleID = @subtitleID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@subtitleID", subtitleID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subtitle = new Subtitle
                                {
                                    SubtitleID = subtitleID,
                                    EpisodeID = reader.GetString(0),
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Subtitle> GetSubtitlesByEpisodeID(string episodeID)
        {
            try
            {
                var subtitles = new List<Subtitle>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select subtitleID, language, status, subtitleURL " +
                                "From subtitle " +
                                "Where episodeID = @episodeID " +
                                "Order By language";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            command.Parameters.AddWithValue("@episodeID", episodeID);
                            while (reader.Read())
                            {
                                subtitles.Add(new Subtitle
                                {
                                    SubtitleID = reader.GetString(0),
                                    EpisodeID = episodeID,
                                    Language = reader.GetString(1),
                                    Status = reader.GetString(2),
                                    SubtitleURL = reader.GetString(3)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return subtitles;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
