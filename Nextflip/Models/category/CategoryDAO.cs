    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using Nextflip.utils;

namespace Nextflip.Models.category
{
    public class CategoryDAO: ICategoryDAO
    { 

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var categories = new List<CategoryDTO>();
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = "Select categoryID, name From category";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new CategoryDTO
                            {
                                CategoryID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return categories;
        }

        public CategoryDTO GetCategoryByID(int categoryID)
        {
            CategoryDTO category = null;
            using (var connection = new MySqlConnection(DbUtil.ConnectionString))
            {
                connection.Open();
                string Sql = $"Select categoryID, name From category Where categoryID = '{categoryID}'";
                using (var command = new MySqlCommand(Sql, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new CategoryDTO
                            {
                                CategoryID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            };
                        }
                    }
                }
                connection.Close();
            }
            return category;            
        }
    }
}
