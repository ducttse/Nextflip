using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Nextflip.Models
{
    public class BaseDAL : DataProvider
    {
        public DataProvider dataProvider { get; set; } = null;
        public SqlConnection connection = null;
        public BaseDAL()
        {
            var connectionString = GetConnectionString();
            dataProvider = new DataProvider(connectionString);

        }

        private string GetConnectionString()
        {
            string connectionString;
            IConfiguration config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", true, true)
                                    .Build();
            connectionString = config["ConnectionString:MySQL"];
            return connectionString;
        }
        public void CloseConnection() => dataProvider.CloseConnection(connection);
    }
}