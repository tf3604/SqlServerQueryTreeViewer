using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    internal class SqlExecuteCompleteEventArgs : EventArgs
    {
        public SqlExecuteCompleteEventArgs()
        {
            Exception = null;
            CancelledByUser = false;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public bool CancelledByUser
        {
            get;
            set;
        }
    }
}
