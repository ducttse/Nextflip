using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models.DTO;
namespace Nextflip.Models.season
{
    public class SeasonDAO:BaseDAL,ISeasonDAO
    {
        public SeasonDAO() { }
        public IEnumerable<SeasonDTO> GetSeasonsByMediaID(string mediaID)
        {
            var seasons = new List<SeasonDTO>();
            IDataReader dataReader = null;
            string Sql = "Select seasonID, status, number " +
                        "From season " +
                        "Where mediaID = @MediaID";
            try
            {
                var param = dataProvider.CreateParameter("@MediaID", 20, mediaID, DbType.String);
                dataReader = dataProvider.GetDataReader(Sql, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    seasons.Add(new SeasonDTO
                    {
                        SeasonID = dataReader.GetString(0),
                        MediaID = mediaID,
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
            return seasons;
        }
    }
}
