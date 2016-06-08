using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    internal abstract class InfoTracker
    {
        protected static void ExecuteNonQuery(SqlConnection connection, string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        protected static DataTable ExecuteQuery(SqlConnection connection, string sql)
        {
            DataTable results = new DataTable();
            try
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(results);
                        return results;
                    }
                }
            }
            catch
            {
                if (results != null)
                {
                    results.Dispose();
                    results = null;
                }
                throw;
            }
        }
    }
}
