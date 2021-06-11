using MySql.Data.MySqlClient;
using Nextflip.utils;
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
            IList<SupportTopic> supportTickets = new List<SupportTopic>();
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT topicName " +
                                "FROM supportTopic ;";
                    using (var command = new MySqlCommand(sql, connection))

                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string topicName = reader.GetString("topicName");
                                Console.WriteLine(topicName);
                                supportTickets.Add(new SupportTopic(topicName));
                            }
                            connection.Close();
                        }
                    }
                }
                return supportTickets;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
