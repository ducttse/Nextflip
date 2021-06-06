using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.DTO;
using System.Data;

namespace Nextflip.Models.DAO
{
    public class CategoryDAO: BaseDAL
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();
            IDataReader dataReader = null;
            string Sql = "Select categoryID, name " +
                        "From category";
            try
            {
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return categories;
        }

        public Category GetCategoryByID(int categoryID)
        {
            var category = new Category();
            IDataReader dataReader = null;
            string Sql = "Select categoryID, name " +
                        "From category " +
                        "Where categoryID = @CategoryID";
            try
            {
                var param = dataProvider.CreateParameter("@CategoryID", 4, categoryID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    category = new Category
                    {
                        CategoryID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return category;
        }
    }
}
