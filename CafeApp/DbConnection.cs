using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApplication
{
    class DbConnection
    {
        private static string connectionString = "Data Source=HP-OMEN;Initial Catalog=CafeDatabase;MultipleActiveResultSets=true;Persist Security Info=True;User ID=Admin;Password=0202";
        private static SqlConnection connection = null;
       private DbConnection() { }
        public static SqlConnection GetConnection()
        {
            if (DbConnection.connection == null)
            {
                DbConnection.connection = new SqlConnection(connectionString);
            }
            return DbConnection.connection;
        }
        public static void Open()
        {
            if (DbConnection.connection == null)
            {
                DbConnection.connection = new SqlConnection(connectionString);
            }
            DbConnection.connection.Open();
        }
        public static void Close()
        {
            if (DbConnection.connection != null)
            {
                DbConnection.connection.Close();
            }
            
        }
        
    }
}
