using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nextflip.utils;

namespace Nextflip.Models.paymentPlan
{
    public class PaymentPlanDAO : IPaymentPlanDAO
    {
        public IList<PaymentPlan> GetPaymentPlan()
        {
            IList<PaymentPlan> result = null;
            try
            {
                using (var connection = new MySqlConnection(DbUtil.ConnectionString))
                {
                    connection.Open();
                    string Sql = @"select paymentPlanID, price, duration " +
                                 "from paymentplan " +
                                 "where paymentPlanID in ( " +
                                 "select max(paymentPlanID) " +
                                 "from paymentplan " +
                                 "group by duration)";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (result == null) result = new List<PaymentPlan>();
                                PaymentPlan paymentPlan = new PaymentPlan()
                                {
                                    PaymentPlanID = reader.GetInt32(0),
                                    Price = reader.GetDecimal(1),
                                    Duration = reader.GetInt32(2)
                                };
                                result.Add(paymentPlan);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
