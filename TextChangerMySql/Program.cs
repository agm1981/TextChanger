using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChangerMySql.MySqlConn;

namespace TextChangerMySql
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlPost pst = new MySqlPost();
            pst.Worker();
        } 
    }
}
