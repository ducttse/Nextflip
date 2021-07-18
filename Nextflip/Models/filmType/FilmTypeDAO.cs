using MySql.Data.MySqlClient;
using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.filmType
{
    public class FilmTypeDAO : IFilmTypeDAO
    {
        public IEnumerable<FilmType> GetFilmTypes()
        {
            try
            {
                var filmTypes = new List<FilmType>();
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select typeID, type " +
                                "From filmType " +
                                "Order By type ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                filmTypes.Add(new FilmType
                                {
                                    TypeID = reader.GetInt32("typeID"),
                                    Type = reader.GetString("type")
                                });
                            }
                        }
                    }
                    connection.Close();
                }
                return filmTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FilmType GetFilmTypeByID(int typeID)
        {
            try
            {
                FilmType filmType = null;
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select typeID, type " +
                                "From filmType " +
                                "Where typeID = @typeID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@typeID", typeID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                filmType = new FilmType
                                {
                                    TypeID = reader.GetInt32("typeID"),
                                    Type = reader.GetString("type")
                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return filmType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdateFilmType(int typeID, string filmType)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    string sql = "Update filmType " +
                                "Set type = @filmType " +
                                "Where typeID = @typeID ";
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@filmType", filmType);
                        command.Parameters.AddWithValue("@typeID", typeID);

                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public bool CreateNewFilmType(string filmTypeName)
        {
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    string sql = "Insert into filmType (type) " +
                                "Value(@filmTypeName)";
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@filmTypeName", filmTypeName);

                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
    }
}
