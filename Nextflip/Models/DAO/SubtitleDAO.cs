using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models.DTO;

namespace Nextflip.Models.DAO
{
    public class SubtitleDAO : BaseDAL
    {
        private static SubtitleDAO instance = null;
        private static readonly object instanceLock = new object();
        private SubtitleDAO() { }
        public static SubtitleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SubtitleDAO();
                    }
                    return instance;
                }
            }
        }

        public Subtitle GetSubtitleByEpisodeID(string episodeID)
        {
            Subtitle subtitle = null;
            IDataReader dataReader = null;
            string Sql = "Select subtitleID, language, status, subtitleURL " +
                        "From Subtitle " +
                        "Where episodeID = @EpisodeID";
            try
            {
                var param = dataProvider.CreateParameter("@EpisodeID", 20, episodeID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    subtitle = new Subtitle
                    {
                        SubtitleID = dataReader.GetString(0),
                        EpisodeID = episodeID,
                        Language = dataReader.GetString(1),
                        Status = dataReader.GetString(2),
                        SubtitleURL = dataReader.GetString(3)
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
            return subtitle;
        }
    }
}
