//  Copyright(c) 2016-2017 Brian Hansen.

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
