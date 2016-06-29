using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlMemoNode
    {
        public SqlMemoNode()
        {
            Identifier = new SqlMemoNodeIdentifier();
            Parents = new List<SqlMemoNode>();
        }

        public SqlMemoNodeIdentifier Identifier
        {
            get;
            private set;
        }

        public string OperationName
        {
            get;
            set;
        }

        public List<SqlMemoNode> Parents
        {
            get;
            private set;
        }

        public string Arguments
        {
            get;
            set;
        }

        public string DisplayOperationName
        {
            get
            {
                string operationName = string.IsNullOrEmpty(OperationName) ? string.Empty : OperationName + " ";
                return operationName;
            }
        }

        public string DisplayParentName
        {
            get
            {
                StringBuilder parentString = new StringBuilder();
                bool isFirst = true;
                foreach (SqlMemoNode parent in Parents)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        parentString.Append(" ");
                    }
                    parentString.Append(parent.Identifier.ToString());
                }

                return parentString.ToString();
            }
        }

        public string DisplayName
        {
            get
            {
                return DisplayOperationName + DisplayParentName;
            }
        }

        public override string ToString()
        {
            return Identifier.ToString() + ": " + DisplayName;
        }
    }
}
