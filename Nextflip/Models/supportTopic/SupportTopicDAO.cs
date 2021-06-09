using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Models.supportTopic
{
    class SupportTopicDAO : ISupportTopicDAO
    {
        public IList<SupportTopic> GetAllTopics()
        {
            IList<SupportTopic> supportTopics = new List<SupportTopic>();
            using (var connection = new MySqlConnection(utils.DbUtil.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT supportTopicID, type " +
                                "FROM supportTopic ;";
                using (var command = new MySqlCommand(sql, connection))

                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string topicName = reader.GetString("type");
                            supportTopics.Add(new SupportTopic { topicName = topicName });
                        }
                        connection.Close();
                    }
                }

            }
            return supportTopics;
        }

    }
}
