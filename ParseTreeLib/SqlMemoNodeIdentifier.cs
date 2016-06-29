using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlMemoNodeIdentifier
    {
        public int GroupNumber
        {
            get;
            set;
        }

        public int OperationNumber
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            SqlMemoNodeIdentifier that = obj as SqlMemoNodeIdentifier;
            if ((object)that == null)
            {
                return false;
            }

            return this.GroupNumber == that.GroupNumber &&
                this.OperationNumber == that.OperationNumber;
        }

        public override int GetHashCode()
        {
            return GroupNumber.GetHashCode() ^ OperationNumber.GetHashCode();
        }

        public override string ToString()
        {
            return GroupNumber.ToString() + "." + OperationNumber.ToString();
        }

        public static bool operator ==(SqlMemoNodeIdentifier node1, SqlMemoNodeIdentifier node2)
        {
            if ((object)node1 == null && (object)node2 == null)
            {
                return true;
            }
            if (((object)node1 == null && (object)node2 != null) ||
                ((object)node1 != null && (object)node2 == null))
            {
                return false;
            }

            return node1.Equals(node2);
        }

        public static bool operator !=(SqlMemoNodeIdentifier node1, SqlMemoNodeIdentifier node2)
        {
            return !(node1 == node2);
        }
    }
}
