using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceConfig.Data
{
    public class DatabaseRepo
    {
        private string _connString = ConfigurationManager.ConnectionStrings["PriceConfig"].ConnectionString;

        public void InsertData(List<string[]> inserts)
        {
            using (var cn = new SqlConnection(_connString))
            {
                var parameters = new DynamicParameters();

                foreach (var price in inserts)
                {
                    parameters.Add("@PriceId", price[0]);
                    parameters.Add("@Price", price[1]);
                    cn.Execute("InsertPrice", parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
        }

        public List<PriceItem> GetPrices()
        {
            using(var cn = new SqlConnection(_connString))
            {
                return cn.Query<PriceItem>("GetAllPrices", commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public void UpdatePrice(int priceId, decimal newPrice)
        {
            using(var cn = new SqlConnection(_connString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PriceId", priceId);
                parameters.Add("@NewPrice", newPrice);
                cn.Execute("UpdatePrice", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public bool DeleteData()
        {
            using(var cn = new SqlConnection(_connString))
            {
                return cn.Execute("DeleteData", commandType: System.Data.CommandType.StoredProcedure) > 0;
            }
        }
    }
}
