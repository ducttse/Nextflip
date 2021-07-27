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
                    string Sql = "Select episodeID, title, thumbnailURL, status, number, episodeURL, note " +
                                "From episode " +
                                "Where seasonID = @seasonID and status != 'Removed' " +
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
                                    EpisodeURL = reader.GetString(5),
                                    Note = reader.IsDBNull(6) ? null : reader.GetString(6)
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
        public IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID,string status)
        {
            try
            {
                var episodes = new List<Episode>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select episodeID, title, thumbnailURL, status, number, episodeURL, note " +
                                "From episode " +
                                "Where seasonID = @seasonID and status = @status " +
                                "Order By number";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID", seasonID);
                        command.Parameters.AddWithValue("@status", status);
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
                                    EpisodeURL = reader.GetString(5),
                                    Note = reader.IsDBNull(6) ? null : reader.GetString(6)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return episodes;
            }
            catch (Exception ex)
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
                    string Sql = "Select title, thumbnailURL, seasonID, status, number, episodeURL, note " +
                                "From episode " +
                                "Where episodeID = @episodeID and status != 'Removed'";
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
                                    EpisodeURL = reader.GetString(5),
                                    Note = reader.IsDBNull(6) ? null : reader.GetString(6)
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
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "approveEpisode";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@episodeID_Input", ID);
                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow == 1) result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool DisapproveChangeEpisode(string ID, string note)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "disapproveEpisode";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("episodeID_Input", ID);
                        command.Parameters.AddWithValue("note_Input", note);
                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow == 1) result = true;
                    }
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

        public bool CheckStatusEpisode(string seasonID)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select exists (select status from episode where seasonID = @seasonID AND status != 'Approved' AND status != 'Removed')";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID", seasonID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) return reader.GetBoolean(0);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;

        }

        public string AddEpisode_Transact(MySqlConnection connection, Episode episode)
        {
            string episodeID = null;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "createEpisode";
            command.Parameters.AddWithValue("@seasonID_Input", episode.SeasonID);
            command.Parameters.AddWithValue("@title_Input", episode.Title);
            command.Parameters.AddWithValue("@thumbnailURL_Input", episode.ThumbnailURL);
            command.Parameters.AddWithValue("@episodeNum_Input", episode.Number);
            command.Parameters.AddWithValue("@episodeURL_Input", episode.EpisodeURL);
            command.Parameters.AddWithValue("@status_Input", episode.Status);
            command.Parameters.Add("@episodeID_Output", MySqlDbType.String).Direction
                = ParameterDirection.Output;
            command.ExecuteNonQuery();
            episodeID = (string)command.Parameters["@episodeID_Output"].Value;
            return episodeID;
        }

        public int RemoveEpisode_Transact(MySqlConnection connection, string episodeId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "removeEpisode";
            command.Parameters.AddWithValue("@episodeID", episodeId);
            return command.ExecuteNonQuery();
        }


    }
}
