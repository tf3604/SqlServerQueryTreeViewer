using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlMemoGroup
    {
        public SqlMemoGroup()
        {
            Operations = new List<SqlMemoNode>();
        }

        public int GroupNumber
        {
            get;
            set;
        }

        public string Arguments
        {
            get;
            set;
        }

        public bool IsRoot
        {
            get;
            set;
        }

        public List<SqlMemoNode> Operations
        {
            get;
            private set;
        }
    }
}
