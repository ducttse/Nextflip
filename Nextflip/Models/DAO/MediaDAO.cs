
using Nextflip.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Nextflip.Models.DAO
{
    public class MediaDAO: BaseDAL
    {
        private static MediaDAO instance = null;
        private static readonly object instanceLock = new object();
        private MediaDAO() { }
        public static MediaDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MediaDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Media> GetMediasByTitle(string searchValue)
        {
            var medias = new List<Media>();
            IDataReader dataReader = null;
            string Sql = "Select mediaID, title, bannerURL, language, description " +
                        "From Media " +
                        "Where title LIKE @Title";
            try
            {
                var param = dataProvider.CreateParameter("@Title", 40, $"%{searchValue}%",DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    medias.Add(new Media
                    {
                        MediaID = dataReader.GetString(0),
                        Title = dataReader.GetString(1),
                        BannerURL = dataReader.GetString(2),
                        Language = dataReader.GetString(3),
                        Description = dataReader.GetString(4),
                    }) ;
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
            return medias;
        }

        public Media GetMediasByID(string mediaID)
        {
            var media = new Media();
            IDataReader dataReader = null;
            string Sql = "Select mediaID, status, title, bannerURL, language, description " +
                        "From Media " +
                        "Where mediaID = @MediaID";
            try
            {
                var param = dataProvider.CreateParameter("@MediaID", 20, mediaID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    media = new Media
                    {
                        MediaID = dataReader.GetString(0),
                        Status = dataReader.GetString(1),
                        Title = dataReader.GetString(2),
                        BannerURL = dataReader.GetString(3),
                        Language = dataReader.GetString(4),
                        Description = dataReader.GetString(5),
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
            return media;
        }
    }
}
