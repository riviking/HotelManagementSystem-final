using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem3
{
    public class DB
    {
        public static string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=hotelDB;Integrated Security=True;";

    public static SqlConnection GetConnection()
        {
            return new SqlConnection(conn);
        }
    }
}
