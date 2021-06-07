
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
            var categoryIDs = new List<int>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select categoryID From mediaCategory Where mediaID =@mediaID";
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

        public IList<string> GetMediaIDs(int categoryID)
        {
            var mediaIDs = new List<string>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = $"Select mediaID From mediaCategory Where categoryID = '{categoryID}'";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader =  command.ExecuteReader())
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
    }
}
