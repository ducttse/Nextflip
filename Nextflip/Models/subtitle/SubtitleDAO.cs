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
                string ID_preview = ID + "_preview";
                string ID_Available = ID + "_Available";
                string ID_Unavailable = ID + "_Unavailable";
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    Subtitle subtitle_preview = GetSubtitleByID(ID_preview);
                    Subtitle subtitle_Available = GetSubtitleByID(ID_Available);
                    Subtitle subtitle_Unavailable = GetSubtitleByID(ID_Unavailable);
                    if (subtitle_preview != null)
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set episodeID = @episodeID, language = @language, subtitleURL = @subtitleURL " +
                            "Where subtitleID = @ID";
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_preview";
                    }
                    if (subtitle_Available != null)
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set status = 'Available' " +
                            "Where subtitleID = @ID";
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_Available";
                    }
                    if (subtitle_Unavailable != null)
                    {
                        SqlUpdate = "Update subtitle " +
                            "Set status = 'Unavailable' " +
                            "Where subtitleID = @ID";
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_Unavailable";
                    }
                    MySqlCommand command1 = new MySqlCommand(SqlUpdate, connection);
                    MySqlCommand command2 = new MySqlCommand(SqlDelete, connection);
                    command1.Parameters.AddWithValue("@ID", ID);
                    if (subtitle_preview != null)
                    {
                        command1.Parameters.AddWithValue("@episodeID", subtitle_preview.EpisodeID);
                        command1.Parameters.AddWithValue("@language", subtitle_preview.Language);
                        command1.Parameters.AddWithValue("@subtitleURL", subtitle_preview.SubtitleURL);
                        command2.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (subtitle_Available != null)
                    {
                        command2.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (subtitle_Unavailable != null)
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

        public bool DisapproveChangeSubtitle(string ID)
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
                    Subtitle subtitle_preview = GetSubtitleByID(ID_preview);
                    Subtitle subtitle_Available = GetSubtitleByID(ID_Available);
                    Subtitle subtitle_Unavailable = GetSubtitleByID(ID_Unavailable);
                    if (subtitle_preview != null)
                    {
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_preview";
                    }
                    if (subtitle_Available != null)
                    {
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_Available";
                    }
                    if (subtitle_Unavailable != null)
                    {
                        SqlDelete = "Delete from subtitle " +
                            "Where subtitleID = @ID_Unavailable";
                    }
                    MySqlCommand command = new MySqlCommand(SqlDelete, connection);
                    if (subtitle_preview != null)
                    {
                        command.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (subtitle_Available != null)
                    {
                        command.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (subtitle_Unavailable != null)
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

        public bool RequestChangeSubtitleStatus(string subtitleID, string newStatus)
        {
            var result = false;
            try
            {
                Subtitle subtitle = GetSubtitleByID(subtitleID);
                if (subtitle.Status.Trim().Equals("Pending")) return false;
                subtitle.SubtitleID = subtitle.SubtitleID + "_" + newStatus;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO subtitle (subtitleID, episodeID, language, status, subtitleURL) " +
                        "VALUES (@subtitleID_preview, @episodeID, @language, 'Pending', @subtitleURL) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@subtitleID_preview", subtitle.SubtitleID);
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
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
