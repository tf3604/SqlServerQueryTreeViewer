using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlParseTreeNode
    {
        public SqlParseTreeNode()
        {
            this.Children = new List<SqlParseTreeNode>();
        }

        public int SequenceNumber
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public string OperationName
        {
            get;
            set;
        }

        public string Arguments
        {
            get;
            set;
        }

        public OperationType Operation
        {
            get;
            set;
        }

        public SqlParseTreeNode Parent
        {
            get;
            set;
        }

        public List<SqlParseTreeNode> Children
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return OperationName;
        }
    }
}
