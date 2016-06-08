using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlParseTree
    {
        public SqlParseTree()
        {
            this.Nodes = new List<SqlParseTreeNode>();
        }

        public string TreeDescription
        {
            get;
            set;
        }

        public List<SqlParseTreeNode> Nodes
        {
            get;
            private set;
        }
    }
}
