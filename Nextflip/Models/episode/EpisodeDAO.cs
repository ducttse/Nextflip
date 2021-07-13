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
                MySqlCommand command1;
                MySqlCommand command2;
                int rowEffects1 = 0;
                int rowEffects2 = 0;
                string episodeID = ID.Split('_')[0];
                Episode episode = new Episode();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (episodeID.Equals(ID))
                    {
                        SqlUpdate = "Update episode " +
                            "Set status = 'Available' " +
                            "Where episodeID = @episodeID";
                    }
                    else
                    {
                        if (ID.Split('_')[1].Trim().ToLower().Equals("preview"))
                        {
                            SqlUpdate = "Update episode " +
                                "Set seasonID = @seasonID, title = @title, thumbnailURL = @thumbnailURL, number = @number, episodeURL = @episodeURL " +
                                "Where episodeID = @episodeID";
                            episode = GetEpisodeByID(ID);
                        }
                        if (ID.Split('_')[1].Trim().ToLower().Equals("available"))
                        {
                            SqlUpdate = "Update episode " +
                                "Set status = 'Available' " +
                                "Where episodeID = @episodeID";
                        }
                        if (ID.Split('_')[1].Trim().ToLower().Equals("unavailable"))
                        {
                            SqlUpdate = "Update episode " +
                                "Set status = 'Unavailable' " +
                                "Where episodeID = @episodeID";
                        }
                        SqlDelete = "Delete from episode " +
                            "Where episodeID = @ID";
                        command2 = new MySqlCommand(SqlDelete, connection);
                        command2.Parameters.AddWithValue("@ID", ID);
                        rowEffects2 = command2.ExecuteNonQuery();
                    }
                    command1 = new MySqlCommand(SqlUpdate, connection);
                    command1.Parameters.AddWithValue("@episodeID", episodeID);
                    if (!episodeID.Equals(ID))
                    {
                        if (ID.Split('_')[1].Trim().ToLower().Equals("preview"))
                        {
                            command1.Parameters.AddWithValue("@title", episode.Title);
                            command1.Parameters.AddWithValue("@seasonID", episode.SeasonID);
                            command1.Parameters.AddWithValue("@thumbnailURL", episode.ThumbnailURL);
                            command1.Parameters.AddWithValue("@number", episode.Number);
                            command1.Parameters.AddWithValue("@episodeURL", episode.EpisodeURL);
                        }
                    }
                    rowEffects1 = command1.ExecuteNonQuery();
                    if (rowEffects1 > 0 && rowEffects2 > 0)
                    {
                        result = true;
                    }
                    if (rowEffects1 > 0 && episodeID.Equals(ID))
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
            string SqlEpisode;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (ID.Split('_')[0].Equals(ID))
                    {
                        SqlEpisode = "Update episode " +
                            "Set status = 'Unavailable' " +
                            "Where episodeID = @ID";
                    } else
                    SqlEpisode = "Delete from episode " +
                            "Where episodeID = @ID";
                    MySqlCommand command = new MySqlCommand(SqlEpisode, connection);
                    command.Parameters.AddWithValue("@ID", ID);
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

        public bool RequestChangeEpisodeStatus(string ID, string newStatus)
        {
            var result = false;
            try
            {
                string episodeID = ID.Split('_')[0];
                Episode episode = GetEpisodeByID(episodeID);
                if (episode.Status.Trim().Equals("Pending")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO episode (episodeID, seasonID, title, thumbnailURL, status, number, episodeURL) " +
                        "VALUES (@episodeID_preview, @seasonID, @title, @thumbnailURL, 'Pending', @number, @episodeURL) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@episodeID_preview", ID);
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
                throw new Exception("fail. This episode is requesting to change status");
            }
            return result;
        }
        public string AddEpisode(Episode episode)
        {
            string episodeID = null;
            try
            {
                MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.CommandText = "createEpisode";
                command.Parameters.AddWithValue("@seasonID_Input", episode.SeasonID);
                command.Parameters.AddWithValue("@title_Input", episode.Title);
                command.Parameters.AddWithValue("@thumbnailURL_Input", episode.ThumbnailURL);
                command.Parameters.AddWithValue("@episodeNum_Input", episode.Number);
                command.Parameters.AddWithValue("@episodeURL_Input", episode.EpisodeURL);
                command.Parameters.Add("@episodeID_Output", MySqlDbType.String).Direction
                    = ParameterDirection.Output;
                command.ExecuteNonQuery();
                episodeID = (string)command.Parameters["@episodeID_Output"].Value;
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail. " + ex.Message);
            }
            return episodeID;
        }

        public string UpdateEpisode(Episode episode)
        {
            string episodeID = null;
            try
            {
                var connection = new MySqlConnection(DbUtil.ConnectionString);
                var command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.CommandText = "updateEpisode";
                command.Parameters.AddWithValue("@episodeID_InOutput", episode.EpisodeID).Direction
                    = ParameterDirection.InputOutput; ;
                command.Parameters.AddWithValue("@title_Input", episode.Title);
                command.Parameters.AddWithValue("@thumbnailURL_Input", episode.ThumbnailURL);
                command.Parameters.AddWithValue("@episodeNum_Input", episode.Number);
                command.Parameters.AddWithValue("@episodeURL_Input", episode.EpisodeURL);
                command.ExecuteNonQuery();
                episodeID = (string)command.Parameters["@episodeID_InOutput"].Value;
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return episodeID;
        }
    }
}
