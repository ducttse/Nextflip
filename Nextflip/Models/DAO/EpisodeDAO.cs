using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models.DTO;

namespace Nextflip.Models.DAO
{
    public class EpisodeDAO : BaseDAL
    {
        private static EpisodeDAO instance = null;
        private static readonly object instanceLock = new object();
        private EpisodeDAO() { }
        public static EpisodeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new EpisodeDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Episode> GetEpisodesBySeasonID(string seasonID)
        {
            var episodes = new List<Episode>();
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
                    episodes.Add(new Episode
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
