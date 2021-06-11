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
    }
}
