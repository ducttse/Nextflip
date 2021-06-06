using System;
using System.Collections.Generic;
using System.Data;

namespace Nextflip.Models.episode
{
    public class EpisodeDAO : BaseDAL, IEpisodeDAO
    {
        public EpisodeDAO() { }
        public IEnumerable<EpisodeDTO> GetEpisodesBySeasonID(string seasonID)
        {
            var episodes = new List<EpisodeDTO>();
            IDataReader dataReader = null;
            string Sql = "Select episodeID, status, number " +
                        "From season " +
                        "Where seasonID = @SeasonID";
            try
            {
                var param = dataProvider.CreateParameter("@SeasonID", 20, seasonID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    episodes.Add(new EpisodeDTO
                    {
                        EpisodeID = dataReader.GetString(0),
                        SeasonID = seasonID,
                        Status = dataReader.GetString(1),
                        Number = dataReader.GetInt32(2)
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
            return episodes;
        }
    }
}
