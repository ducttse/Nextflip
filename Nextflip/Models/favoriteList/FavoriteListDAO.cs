using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Nextflip.Models.favoriteList
{
    public class FavoriteListDAO : BaseDAL, IFavoriteListDAO
    {
        public FavoriteListDAO() { }

        public FavoriteListDTO GetFavoriteList(string userID)
        {
            FavoriteListDTO favoriteList = null;
            IDataReader dataReader = null;
            string Sql = "Select favoriteListID " +
                        "From favoriteList " +
                        "Where userID = @UserID";
            try
            {
                var param = dataProvider.CreateParameter("@UserID", 20, userID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    favoriteList = new FavoriteListDTO
                    {
                        FavoriteListID = dataReader.GetString(0),
                        UserID = userID,
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
            return favoriteList;
        }
    }
}
