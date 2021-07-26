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
                    string Sql = "Select seasonID, title, thumbnailURL, status, number, note " +
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
                                    Number = reader.GetInt32(4),
                                    Note = reader.IsDBNull(5) ? null : reader.GetString(5)
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
        public IEnumerable<Season> GetSeasonsByMediaID(string mediaID,string status)
        {
            try
            {
                var seasons = new List<Season>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select seasonID, title, thumbnailURL, status, number " +
                                "From season " +
                                "Where mediaID = @mediaID And Status = @status " +
                                "Order By number";

                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        command.Parameters.AddWithValue("@status", status);
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
                                    Number = reader.GetInt32(4),
                                    Note = reader.IsDBNull(5) ? null : reader.GetString(5)
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return seasons;
            }
            catch (Exception ex)
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
                    string Sql = "Select  title, thumbnailURL, mediaID, status, number, note " +
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
                                    Number = reader.GetInt32(4),
                                    Note = reader.IsDBNull(5) ? null : reader.GetString(5)
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
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "approveSeason";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("seasonID_Input", ID);
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

        public bool DisapproveChangeSeason(string ID, string note)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "disapproveSeason";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("seasonID_Input", ID);
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

        public bool RequestChangeSeasonStatus(string ID, string newStatus)
        {
            var result = false;
            try
            {
                string seasonID = ID.Split('_')[0];
                Season season = GetSeasonByID(seasonID);
                if (season.Status.Trim().Equals("Pending")) return false;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO season (seasonID, mediaID, title, thumbnailURL, status, number) " +
                        "VALUES (@seasonID_preview, @mediaID, @title, @thumbnailURL, 'Pending', @number) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@seasonID_preview", ID);
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
                throw new Exception("fail. This season is requesting to change status");
            }
            return result;
        }
        public string AddSeason(Season season)
        {
            string seasonID = null;
            try
            {
                MySqlConnection connection = new MySqlConnection(DbUtil.ConnectionString);
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.CommandText = "createSeason";
                command.Parameters.AddWithValue("@mediaID_Input", season.MediaID);
                command.Parameters.AddWithValue("@title_Input", season.Title);
                command.Parameters.AddWithValue("@thumbnailURL_Input", season.ThumbnailURL);
                command.Parameters.AddWithValue("@seasonNum_Input", season.Number);
                command.Parameters.Add("@seasonID_Output", MySqlDbType.String).Direction
                    = ParameterDirection.Output;
                command.ExecuteNonQuery();
                seasonID = (string)command.Parameters["@seasonID_Output"].Value;
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail. " + ex.Message);
            }
            return seasonID;
        }

        public string UpdateSeason(Season season)
        {
            string seasonID = null;
            try
            {
                var connection = new MySqlConnection(DbUtil.ConnectionString);
                var command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.CommandText = "updateSeason";
                command.Parameters.AddWithValue("@seasonID_InOutput", season.SeasonID).Direction
                     = ParameterDirection.InputOutput;
                command.Parameters.AddWithValue("@title_Input", season.Title);
                command.Parameters.AddWithValue("@thumbnailURL_Input", season.ThumbnailURL);
                command.Parameters.AddWithValue("@seasonNum_Input", season.Number);
                command.ExecuteNonQuery();
                seasonID = (string)command.Parameters["@seasonID_InOutput"].Value;
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return seasonID;
        }


        public string AddSeason_Transact(MySqlConnection connection, Season season)
        {
            string seasonID = null;
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "createSeason";
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "createSeason";
            command.Parameters.AddWithValue("@mediaID_Input", season.MediaID);
            command.Parameters.AddWithValue("@title_Input", season.Title);
            command.Parameters.AddWithValue("@thumbnailURL_Input", season.ThumbnailURL);
            command.Parameters.AddWithValue("@seasonNum_Input", season.Number);
            command.Parameters.AddWithValue("@status_Input", season.Status);
            command.Parameters.Add("@seasonID_Output", MySqlDbType.String).Direction
                = ParameterDirection.Output;
            command.ExecuteNonQuery();
            seasonID = (string)command.Parameters["@seasonID_Output"].Value;
            return seasonID;
        }

        public int RemoveSeason_Transact(MySqlConnection connection, string seasonId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "removeSeason";
            command.Parameters.AddWithValue("@seasonID", seasonId);
            return command.ExecuteNonQuery();
        }

        public bool CheckStatusSeason(string mediaID)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select exists (select status from season where mediaID = @mediaID AND status != 'Approved' AND status != 'Removed')";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) return reader.GetBoolean(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;

        }

        public int NumberAvailableSeason(string mediaID)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(mediaID) FROM Nextflip.season " +
                                  " WHERE mediaID = @mediaID and status != 'Removed' " +
                                  " GROUP BY(mediaID)";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) return reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }
    }
}
