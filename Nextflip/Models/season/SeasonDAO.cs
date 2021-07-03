using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.season
{
    public class SeasonDAO: ISeasonDAO
    {
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID)
        {
            try
            {
                var seasons = new List<Season>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select seasonID, title, thumbnailURL, status, number " +
                                "From season " +
                                "Where mediaID = @mediaID " +
                                "Order By number";

                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                seasons.Add(new Season
                                {
                                    SeasonID = reader.GetString(0),
                                    Title = reader.GetString(1),
                                    ThumbnailURL = reader.GetString(2),
                                    MediaID = mediaID,
                                    Status = reader.GetString(3),
                                    Number = reader.GetInt32(4)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return seasons;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Season GetSeasonByID(string seasonID)
        {
            try
            {
                Season season = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select  title, thumbnailURL, mediaID, status, number " +
                                "From season " +
                                "Where seasonID = @seasonID";

                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID", seasonID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                season= new Season
                                {
                                    SeasonID = seasonID,
                                    Title = reader.GetString(0),
                                    ThumbnailURL = reader.GetString(1),
                                    MediaID = reader.GetString(2),
                                    Status = reader.GetString(3),
                                    Number = reader.GetInt32(4)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return season;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ApproveChangeSeason(string ID)
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
                    Season season_preview = GetSeasonByID(ID_preview);
                    Season season_Available = GetSeasonByID(ID_Available);
                    Season season_Unavailable = GetSeasonByID(ID_Unavailable);
                    if (season_preview != null)
                    {
                        SqlUpdate = "Update season " +
                            "Set mediaID = @mediaID, title = @title, thumbnailURL = @thumbnailURL, number = @number " +
                            "Where seasonID = @ID";
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_preview";
                    }
                    if (season_Available != null)
                    {
                        SqlUpdate = "Update season " +
                            "Set status = 'Available' " +
                            "Where seasonID = @ID";
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_Available";
                    }
                    if (season_Unavailable != null)
                    {
                        SqlUpdate = "Update season " +
                            "Set status = 'Unavailable' " +
                            "Where seasonID = @ID";
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_Unavailable";
                    }
                    MySqlCommand command1 = new MySqlCommand(SqlUpdate, connection);
                    MySqlCommand command2 = new MySqlCommand(SqlDelete, connection);
                    command1.Parameters.AddWithValue("@ID", ID);
                    if (season_preview != null)
                    {
                        command1.Parameters.AddWithValue("@title", season_preview.Title);
                        command1.Parameters.AddWithValue("@mediaID", season_preview.MediaID);
                        command1.Parameters.AddWithValue("@thumbnailURL", season_preview.ThumbnailURL);
                        command1.Parameters.AddWithValue("@number", season_preview.Number);
                        command2.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (season_Available != null)
                    {
                        command2.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (season_Unavailable != null)
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

        public bool DisapproveChangeSeason(string ID)
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
                    Season season_preview = GetSeasonByID(ID_preview);
                    Season season_Available = GetSeasonByID(ID_Available);
                    Season season_Unavailable = GetSeasonByID(ID_Unavailable);
                    if (season_preview != null)
                    {
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_preview";
                    }
                    if (season_Available != null)
                    {
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_Available";
                    }
                    if (season_Unavailable != null)
                    {
                        SqlDelete = "Delete from season " +
                            "Where seasonID = @ID_Unavailable";
                    }
                    MySqlCommand command = new MySqlCommand(SqlDelete, connection);
                    if (season_preview != null)
                    {
                        command.Parameters.AddWithValue("@ID_preview", ID_preview);
                    }
                    if (season_Available != null)
                    {
                        command.Parameters.AddWithValue("@ID_Available", ID_Available);
                    }
                    if (season_Unavailable != null)
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

        public bool RequestChangeSeasonStatus(string seasonID, string newStatus)
        {
            var result = false;
            try
            {
                Season season = GetSeasonByID(seasonID);
                if (season.Status.Trim().Equals("Pending")) return false;
                season.SeasonID = season.SeasonID + "_" + newStatus;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO season (seasonID, mediaID, title, thumbnailURL, status, number) " +
                        "VALUES (@seasonID_preview, @mediaID, @title, @thumbnailURL, 'Pending', @number) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID_preview", season.SeasonID);
                        command.Parameters.AddWithValue("@mediaID", season.MediaID);
                        command.Parameters.AddWithValue("@title", season.Title);
                        command.Parameters.AddWithValue("@thumbnailURL", season.ThumbnailURL);
                        command.Parameters.AddWithValue("@number", season.Number);
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
