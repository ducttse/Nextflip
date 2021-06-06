
using System;
using System.Collections.Generic;
using System.Data;

namespace Nextflip.Models.mediaCategory
{
    public class MediaCategoryDAO : BaseDAL, IMediaCategoryDAO
    {
        public MediaCategoryDAO() { }

        public List<int> GetCategoryIDs(string mediaID)
        {
            var categoryIDs = new List<int>();
            IDataReader dataReader = null;
            string Sql = "Select categoryID " +
                        "From MediaCategory " +
                        "Where mediaID = @MediaID";
            try
            {
                var param = dataProvider.CreateParameter("@MediaID", 20, mediaID,DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    categoryIDs.Add(dataReader.GetInt32(0));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return categoryIDs;
        }

        public List<string> GetMediaIDs(int categoryID)
        {
            var mediaIDs = new List<string>();
            IDataReader dataReader = null;
            string Sql = "Select mediaID " +
                        "From MediaCategory " +
                        "Where categoryID = @CategoryID";
            try
            {
                var param = dataProvider.CreateParameter("@CategoryID", 4, categoryID, DbType.Int32);
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
