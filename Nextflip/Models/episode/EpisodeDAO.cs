using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.episode
{
    public class EpisodeDAO : IEpisodeDAO
    {

        public IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID)
        {
            try
            {
                var episodes = new List<Episode>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select episodeID, title, thumbnailURL, status, number, episodeURL From episode Where seasonID = @seasonID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID", seasonID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                episodes.Add(new Episode
                                {
                                    EpisodeID = reader.GetString(0),
                                    Title = reader.GetString(1),
                                    ThumbnailURL = reader.GetString(2),
                                    SeasonID = seasonID,
                                    Status = reader.GetString(3),
                                    Number = reader.GetInt32(4),
                                    EpisodeURL = reader.GetString(5)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return episodes;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Episode GetEpisodeByID(string episodeID)
        {
            try
            {
                Episode episode = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select title, thumbnailURL, seasonID, status, number, episodeURL From episode Where episodeID = @episodeID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@episodeID", episodeID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                episode = new Episode
                                {
                                    EpisodeID = episodeID,
                                    Title = reader.GetString(0),
                                    ThumbnailURL = reader.GetString(1),
                                    SeasonID = reader.GetString(2),
                                    Status = reader.GetString(3),
                                    Number = reader.GetInt32(4),
                                    EpisodeURL = reader.GetString(5)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return episode;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
