using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models;

namespace Nextflip.Models.subtitle
{
    public class SubtitleDAO : BaseDAL,ISubtitleDAO
    {
        public SubtitleDAO() { }

        public SubtitleDTO GetSubtitleByEpisodeID(string episodeID)
        {
            SubtitleDTO subtitle = null;
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
                    subtitle = new SubtitleDTO
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
