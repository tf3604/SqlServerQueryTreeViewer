using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class Dal : IDisposable
    {
        private ApplicationSqlConnection _connection;
        private SqlCommand _currentCommand;
        private object _commandLocker = new object();

        public Dal(ApplicationSqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            _connection = connection;
        }

        public void ExecuteQueryNoResultSets(string sql, params SqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException(nameof(sql));
            }

            using (SqlCommand command = GetCommand(sql, parameters))
            {
                command.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQueryOneResultSet(string sql, params SqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException(nameof(sql));
            }

            using (SqlCommand command = GetCommand(sql, parameters))
            {
                lock(_commandLocker)
                {
                    _currentCommand = command;
                }
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        try
                        {
                            adapter.Fill(table);
                            return table;
                        }
                        catch
                        {
                            if (table != null)
                            {
                                table.Dispose();
                            }
                            throw;
                        }
                    }
                }
                finally
                {
                    lock(_commandLocker)
                    {
                        _currentCommand = null;
                    }
                }
            }
        }

        public DataSet ExecuteQueryMultipleResultSets(string sql, params SqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException(nameof(sql));
            }

            try
            {
                using (SqlCommand command = GetCommand(sql, parameters))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet set = new DataSet();
                        try
                        {
                            adapter.Fill(set);
                            return set;
                        }
                        catch
                        {
                            if (set != null)
                            {
                                set.Dispose();
                            }
                            throw;
                        }
                    }
                }
            }
            catch (SqlException ex) when (ex.Class == 11 && ex.Number == 0)
            {
                // User cancelled query.
                return new DataSet();
            }
        }

        public void Cancel()
        {
            lock(_commandLocker)
            {
                if (_currentCommand != null)
                {
                    _currentCommand.Cancel();
                    _currentCommand = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_connection != null)
                //{
                //    _connection.Dispose();
                //    _connection = null;
                //}
            }
        }

        private SqlCommand GetCommand(string sql, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(sql, _connection.Connection);
            try
            {
                if (parameters != null &&
                    parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.CommandTimeout = 0;
                _currentCommand = command;
                return command;
            }
            catch
            {
                if (command != null)
                {
                    command.Dispose();
                }
                throw;
            }
        }
    }
}
