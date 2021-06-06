using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Nextflip.Models.DTO;

namespace Nextflip.Models.DAO
{
    public class FavoriteListDAO : BaseDAL
    {
        private static FavoriteListDAO instance = null;
        private static readonly object instanceLock = new object();
        private FavoriteListDAO() { }
        public static FavoriteListDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FavoriteListDAO();
                    }
                    return instance;
                }
            }
        }

        public FavoriteList GetFavoriteList(string userID)
        {
            FavoriteList favoriteList = null;
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
                    favoriteList = new FavoriteList
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
