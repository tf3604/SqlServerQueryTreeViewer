using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class ApplicationSqlConnection : IDisposable
    {
        private SqlConnection _connection;        
        private SqlInfoMessageEventHandler _infoMessage;

        public event SqlInfoMessageEventHandler InfoMessage
        {
            add
            {
            }
            remove
            {
            }
        }

        public ApplicationSqlConnection(SqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            _connection = connection;
        }

        public SqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return _connection != null &&
                    _connection.State == ConnectionState.Open;
            }
        }

        public bool FireInfoMessageEventOnUserErrors
        {
            get
            {
                return _connection.FireInfoMessageEventOnUserErrors;
            }
            set
            {
                _connection.FireInfoMessageEventOnUserErrors = value;
            }
        }

        public void Close()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}
