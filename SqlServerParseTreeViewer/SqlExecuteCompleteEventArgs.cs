using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    internal class SqlExecuteCompleteEventArgs : EventArgs
    {
        private Exception _exception;
    }
}
