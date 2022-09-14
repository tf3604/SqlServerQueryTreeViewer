//  Copyright(c) 2016-2022 Breanna Hansen.

//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.

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
