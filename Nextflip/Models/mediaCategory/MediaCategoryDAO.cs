﻿
using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaCategory
{
    public class MediaCategoryDAO : IMediaCategoryDAO
    {
        public IList<int> GetCategoryIDs(string mediaID)
        {
            try
            {
                var categoryIDs = new List<int>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select categoryID " +
                                "From mediaCategory " +
                                "Where mediaID =@mediaID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryIDs.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    connection.Close();
                }
                return categoryIDs;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IList<string> GetMediaIDs(int categoryID)
        {
            try
            {
                var mediaIDs = new List<string>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select mediaID " +
                                "From mediaCategory " +
                                "Where categoryID = @categoryID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@categoryID", categoryID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mediaIDs.Add(reader.GetString(0));
                            }
                        }
                    }
                    connection.Close();
                }
                return mediaIDs;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddMediaCategory(string mediaID, int categoryID)
        {
            var result = false;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO mediaCategory (mediaID, categoryID) " +
                        "VALUES (@mediaID, @categoryID) ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@mediaID", mediaID);
                        command.Parameters.AddWithValue("@categoryID", categoryID);
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
                throw new Exception("Add mediaCategory fail");
            }
            return result;
        }
    }
}
