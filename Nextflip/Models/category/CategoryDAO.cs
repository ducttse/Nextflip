using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace Nextflip.Models.category
{
    public class CategoryDAO: BaseDAL, ICategoryDAO
    {
        public CategoryDAO() { }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var categories = new List<CategoryDTO>();
            IDataReader dataReader = null;
            string Sql = "Select categoryID, name " +
                        "From category";
            try
            {
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    categories.Add(new CategoryDTO
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

        public CategoryDTO GetCategoryByID(int categoryID)
        {
            var category = new CategoryDTO();
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
                    category = new CategoryDTO
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
