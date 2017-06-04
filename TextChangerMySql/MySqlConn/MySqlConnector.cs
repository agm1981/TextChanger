using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TextChangerMySql.MySqlConn
{
    public class MySqlConnector
    {

        private MySqlConnector()
        {
        }



        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static MySqlConnector _instance = null;
        public static MySqlConnector Instance()
        {
            if (_instance == null)
                _instance = new MySqlConnector();
            return _instance;
        }

        public bool IsConnect()
        {

            if (Connection == null || Connection.State == ConnectionState.Closed)
            {
                string connstring = ConfigurationManager.AppSettings["connString"];
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return Connection?.State == ConnectionState.Open;
        }

        public void Close()
        {
            connection.Close();
        }
    }

}
