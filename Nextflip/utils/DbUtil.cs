using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Nextflip.utils
{
    public static class DbUtil
    {
        public static string ConnectionString { get; set; }

        public static void TestConnection()
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT fullname FROM account", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine(reader.GetValue(0));
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
