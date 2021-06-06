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

        public static async void TestConnection()
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync(); ///tech: connection pooling ado.net
                using (var command = new MySqlCommand("SELECT fullname FROM account", connection))
                {
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetValue(0));
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
