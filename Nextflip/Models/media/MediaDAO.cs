using System;
using System.Collections.Generic;
using System.Data;

namespace Nextflip.Models.media
{
    public class MediaDAO: BaseDAL, IMediaDAO
    {
        public MediaDAO() { }

        public IEnumerable<MediaDTO> GetMediasByTitle(string searchValue)
        {
            var medias = new List<MediaDTO>();
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
                    medias.Add(new MediaDTO
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

        public MediaDTO GetMediasByID(string mediaID)
        {
            var media = new MediaDTO();
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
                    media = new MediaDTO
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
