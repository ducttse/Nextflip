using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.mediaFavorite
{
    public class MediaFavoriteDAO : BaseDAL, IMediaFavoriteDAO
    {
        public MediaFavoriteDAO() { }

        public IList<string> GetMediaIDs(string favoriteListID)
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
