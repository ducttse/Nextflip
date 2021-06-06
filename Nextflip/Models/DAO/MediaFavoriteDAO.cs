using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.DAO
{
    public class MediaFavoriteDAO : BaseDAL
    {
        private static MediaFavoriteDAO instance = null;
        private static readonly object instanceLock = new object();
        private MediaFavoriteDAO() { }
        public static MediaFavoriteDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MediaFavoriteDAO();
                    }
                    return instance;
                }
            }
        }

        public List<string> GetMediaIDs(string favoriteListID)
        {
            var mediaIDs = new List<string>();
            IDataReader dataReader = null;
            string Sql = "Select mediaID " +
                        "From MediaCategory " +
                        "Where favoriteListID = @FavoriteListID";
            try
            {
                var param = dataProvider.CreateParameter("@CategoryID", 20, favoriteListID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    mediaIDs.Add(dataReader.GetString(0));
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
            return mediaIDs;
        }
    }
}
