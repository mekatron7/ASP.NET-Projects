using DVDLibraryWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace DVDLibraryWebAPI.Models
{
    public class ADONETRepo : IDVDRepo
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DVDCatalog"].ConnectionString;

        public void Create(DVDView dvd)
        {
            using(var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;

                var parameters = new DynamicParameters();

                parameters.Add("@DVDId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Title", dvd.Title);
                parameters.Add("@Director", dvd.Director);
                parameters.Add("@ReleaseYear", dvd.ReleaseYear);
                parameters.Add("@Rating", dvd.Rating);
                parameters.Add("@Notes", dvd.Notes);

                cn.Execute("DVDAdd", parameters, commandType: CommandType.StoredProcedure);

                dvd.DVDId = parameters.Get<int>("@DVDId");
            }
        }

        public bool Delete(int id)
        {
            using(var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;

                var parameters = new DynamicParameters();
                
                parameters.Add("@DVDId", id);

                return cn.Execute("DVDRemove", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<DVDView> GetAll()
        {
            using(var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;

                var list = cn.Query<DVDView>("DVDGetAll", commandType: CommandType.StoredProcedure).ToList();

                return list;
            }
        }

        public List<DVDView> GetSearch(string category, string term)
        {
            var results = new List<DVDView>();

            using(var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;

                var parameters = new DynamicParameters();

                parameters.Add("@Search", term);
                
                if (category == "director")
                {
                    results = cn.Query<DVDView>("DVDSelectDirector", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                else if (category == "title")
                {
                    results = cn.Query<DVDView>("DVDSelectTitle", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                else if (category == "rating")
                {
                    results = cn.Query<DVDView>("DVDSelectRating", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                else
                {
                    results = cn.Query<DVDView>("DVDSelectYear", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return results;
        }

        public void Update(DVDView updatedDVD)
        {
            using(var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;

                var parameters = new DynamicParameters();

                parameters.Add("@DVDId", updatedDVD.DVDId);
                parameters.Add("@Title", updatedDVD.Title);
                parameters.Add("@Rating", updatedDVD.Rating);
                parameters.Add("@Director", updatedDVD.Director);
                parameters.Add("@ReleaseYear", updatedDVD.ReleaseYear);
                parameters.Add("@Notes", updatedDVD.Notes);

                cn.Execute("DVDEdit", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}