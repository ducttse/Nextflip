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

        public IEnumerable<Category> GetCategories()
        {
            try
            {
                var categories = new List<Category>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select categoryID, name " +
                                "From category " +
                                "Order By name ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new Category
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Category GetCategoryByID(int categoryID)
        {
            try
            {
                Category category = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select categoryID, name " +
                                "From category " +
                                "Where categoryID = @categoryID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@categoryID", categoryID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                category = new Category
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateCategory(int categoryID, string newCategoryName)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    string sql = "Update category " +
                                "Set name = @categoryName " +
                                "Where categoryID = @categoryID ";
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@categoryName", newCategoryName);
                        command.Parameters.AddWithValue("@categoryID", categoryID);

                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
              }
              catch (Exception ex){
                throw new Exception(ex.Message);
              }
            return false;
        }
        
        public Category GetCategoryByName(string categoryName)
        {
            try
            {
                Category category = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select categoryID, name " +
                                "From category " +
                                "Where categoryName = @categoryName ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@categoryName", categoryName);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                category = new Category
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          }


        public bool CreateNewCategory(string categoryName)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    string sql = "Insert into category (name) " +
                                "Value(@categoryName)";
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@categoryName", categoryName);

                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;

        }
    }
}
