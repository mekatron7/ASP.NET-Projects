using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gematrianator.Data
{
    public class WordsRepo
    {
        private string connString = ConfigurationManager.ConnectionStrings["GematriaDB"].ConnectionString;

        public void AddWord(string word)
        {
            using( var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Word", word);
                parameters.Add("@DateAdded", DateTime.Now);

                cn.Execute("AddWord", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddWordCipher(string word, string cipherId, int value)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Word", word);
                parameters.Add("@CipherID", cipherId);
                parameters.Add("@Value", value);

                cn.Execute("AddWordCipher", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Word> GetWords(int number)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Value", number);

                return cn.Query<Word>("GetWords", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<WordCipher> GetWordCiphers(string word)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Word", word);

                return cn.Query<WordCipher>("GetWordCiphers", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Cipher> GetAllCiphers()
        {
            using (var cn = new SqlConnection(connString))
            {
                return cn.Query<Cipher>("GetAllCiphers", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public bool DeleteWord(string word)
        {
            using (var cn = new SqlConnection(connString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Word", word);

                return cn.Execute("DeleteWord", parameters, commandType: CommandType.StoredProcedure) > 0;
            }
        }
    }
}
