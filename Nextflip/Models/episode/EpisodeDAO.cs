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
                    string Sql = "Select episodeID, title, thumbnailURL, status, number, episodeURL " +
                                "From episode " +
                                "Where seasonID = @seasonID " +
                                "Order By number";
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
                    string Sql = "Select title, thumbnailURL, seasonID, status, number, episodeURL " +
                                "From episode " +
                                "Where episodeID = @episodeID";
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

        public bool ApproveChangeEpisode(string ID)
        {
            var result = false;
            try
            {
                string SqlUpdate = null;
                string SqlDelete = null;
                string ID_preview = ID + "_preview";
                string ID_Available = ID + "_Available";
                string ID_Unavailable = ID + "_Unavailable";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    Episode episode_preview = GetEpisodeByID(ID_preview);
                    Episode episode_Available = GetEpisodeByID(ID_Available);
                    Episode episode_Unavailable = GetEpisodeByID(ID_Unavailable);
                    if (episode_preview != null)
                    {
                        SqlUpdate = "Update episode " +
                            "Set seasonID = @seasonID, title = @title, thumbnailURL = @thumbnailURL, number = @number, episodeURL = @episodeURL " +
                            "Where episodeID = @ID";
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID_preview";
                    }
                    if (episode_Available != null)
                    {
                        SqlUpdate = "Update episode " +
                            "Set status = 'Available' " +
                            "Where episodeID = @ID";
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID_Available";
                    }
                    if (episode_Unavailable != null)
                    {
                        SqlUpdate = "Update episode " +
                            "Set status = 'Unavailable' " +
                            "Where episodeID = @ID";
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID_Unavailable";
                    }
                    MySqlCommand command1 = new MySqlCommand(SqlUpdate, connection);
                    MySqlCommand command2 = new MySqlCommand(SqlDelete, connection);
                    command1.Parameters.AddWithValue("@ID", ID);
                    if (episode_preview != null)
                    {
                        command1.Parameters.AddWithValue("@title", episode_preview.Title);
                        command1.Parameters.AddWithValue("@seasonID", episode_preview.SeasonID);
                        command1.Parameters.AddWithValue("@thumbnailURL", episode_preview.ThumbnailURL);
                        command1.Parameters.AddWithValue("@number", episode_preview.Number);
                        command1.Parameters.AddWithValue("@episodeURL", episode_preview.EpisodeURL);
                        command2.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (episode_Available != null)
                    {
                        command2.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (episode_Unavailable != null)
                    {
                        command2.Parameters.AddWithValue("@ID_Unavailable", ID_Unavailable);
                    }
                    int rowEffects1 = command1.ExecuteNonQuery();
                    int rowEffects2 = command2.ExecuteNonQuery();
                    if (rowEffects1 > 0 && rowEffects2 > 0)
                    {
                        result = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool DisapproveChangeEpisode(string ID)
        {
            var result = false;
            try
            {
                string SqlDelete = null;
                string ID_preview = ID + "_preview";
                string ID_Available = ID + "_Available";
                string ID_Unavailable = ID + "_Unavailable";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    Episode episode_preview = GetEpisodeByID(ID_preview);
                    Episode episode_Available = GetEpisodeByID(ID_Available);
                    Episode episode_Unavailable = GetEpisodeByID(ID_Unavailable);
                    if (episode_preview != null)
                    {
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID_preview";
                    }
                    if (episode_Available != null)
                    {
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID_Available";
                    }
                    if (episode_Unavailable != null)
                    {
                        SqlDelete = "Delete from media " +
                            "Where episodeID = @ID_Unavailable";
                    }
                    MySqlCommand command = new MySqlCommand(SqlDelete, connection);
                    if (episode_preview != null)
                    {
                        command.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (episode_Available != null)
                    {
                        command.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (episode_Unavailable != null)
                    {
                        command.Parameters.AddWithValue("@ID_Unavailable", ID_Unavailable);
                    }
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        result = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool RequestChangeEpisodeStatus(string episodeID, string newStatus)
        {
            var result = false;
            try
            {
                Episode episode = GetEpisodeByID(episodeID);
                if (episode.Status.Trim().Equals("Pending")) return false;
                episode.EpisodeID = episode.EpisodeID + "_" + newStatus;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO episode (episodeID, seasonID, title, thumbnailURL, status, number, episodeURL) " +
                        "VALUES (@episodeID_preview, @seasonID, @title, @thumbnailURL, 'Pending', @number, @episodeURL) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@episodeID_preview", episode.EpisodeID);
                        command.Parameters.AddWithValue("@seasonID", episode.SeasonID);
                        command.Parameters.AddWithValue("@title", episode.Title);
                        command.Parameters.AddWithValue("@thumbnailURL", episode.ThumbnailURL);
                        command.Parameters.AddWithValue("@number", episode.Number);
                        command.Parameters.AddWithValue("@episodeURL", episode.EpisodeURL);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
