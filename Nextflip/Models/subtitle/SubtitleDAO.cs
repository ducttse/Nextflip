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


        public bool ApproveChangeSubtitle(string ID)
        {
            var result = false;
            try
            {
                string SqlUpdate = null;
                string SqlDelete = null;
                string subtitleID = ID.Split('_')[0];
                Subtitle subtitle = new Subtitle();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    if (ID.Split('_')[1].Trim().ToLower().Equals("preview"))
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set episodeID = @episodeID, language = @language, subtitleURL = @subtitleURL " +
                            "Where subtitleID = @subtitleID";
                        subtitle = GetSubtitleByID(ID);
                    }
                    if (ID.Split('_')[1].Trim().ToLower().Equals("available"))
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set status = 'Available' " +
                            "Where subtitleID = @subtitleID";
                    }
                    if (ID.Split('_')[1].Trim().ToLower().Equals("unavailable"))
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set status = 'Unavailable' " +
                            "Where subtitleID = @subtitleID";
                    }
                    SqlDelete = "Delete from subtitle " +
                        "Where subtitleID = @ID";
                    MySqlCommand command1 = new MySqlCommand(SqlUpdate, connection);
                    MySqlCommand command2 = new MySqlCommand(SqlDelete, connection);
                    command1.Parameters.AddWithValue("@subtitleID", subtitleID);
                    command2.Parameters.AddWithValue("@ID", ID);
                    if (ID.Split('_')[1].Trim().ToLower().Equals("preview"))
                    {
                        command1.Parameters.AddWithValue("@episodeID", subtitle.EpisodeID);
                        command1.Parameters.AddWithValue("@language", subtitle.Language);
                        command1.Parameters.AddWithValue("@subtitleURL", subtitle.SubtitleURL);
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

        public bool DisapproveChangeSubtitle(string ID)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string SqlDelete = "Delete from subtitle " +
                        "Where subtitleID = @ID";
                    MySqlCommand command = new MySqlCommand(SqlDelete, connection);
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

        public bool RequestChangeSubtitleStatus(string ID, string newStatus)
        {
            var result = false;
            try
            {
                string subtitleID = ID.Split('_')[0];
                Subtitle subtitle = GetSubtitleByID(subtitleID);
                if (subtitle.Status.Trim().Equals("Pending")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO subtitle (subtitleID, episodeID, language, status, subtitleURL) " +
                        "VALUES (@subtitleID_preview, @episodeID, @language, 'Pending', @subtitleURL) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@subtitleID_preview", ID);
                        command.Parameters.AddWithValue("@episodeID", subtitle.EpisodeID);
                        command.Parameters.AddWithValue("@language", subtitle.Language);
                        command.Parameters.AddWithValue("@subtitleURL", subtitle.SubtitleURL);
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
                throw new Exception("fail. This subtitle is requesting to change status");
            }
            return result;
        }
    }
}
