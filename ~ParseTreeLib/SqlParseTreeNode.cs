using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlParseTreeNode
    {
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

        public int OperationName
        {
            get;
            set;
        }

        public int Arguments
        {
            get;
            set;
        }

        public OperationType Operation
        {
            get;
            set;
        }
    }
}
