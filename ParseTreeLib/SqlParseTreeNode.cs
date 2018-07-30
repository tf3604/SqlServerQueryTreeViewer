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
            if (OperationName == "LogOp_Get")
            {
                string tableName = ExtractTableNameAndAlias();
                return $"Get {tableName}";
            }

            return OperationName;
        }

        private string ExtractTableNameAndAlias()
        {
            // Quick and dirty; probably has some cases where this isn't going to work
            if (string.IsNullOrEmpty(Arguments))
            {
                return string.Empty;
            }

            string tableLabel = "TBL: ";
            string aliasLabel = "alias TBL: ";

            int tableLabelIndex = Arguments.IndexOf(tableLabel, StringComparison.InvariantCultureIgnoreCase);
            if (tableLabelIndex < 0)
            {
                return string.Empty;
            }

            bool inEscape = false;
            bool hasAlias = false;
            int aliasStartIndex = 0;
            StringBuilder sb = new StringBuilder();

            for(int index = tableLabelIndex + tableLabel.Length; index < Arguments.Length; index++)
            {
                char c = Arguments[index];
                if (c == '[')
                {
                    inEscape = true;
                }
                if (c == ']' && inEscape == true)
                {
                    inEscape = false;
                }
                if (c == ' ' && inEscape == false)
                {
                    break;
                }
                if (c == '(' && inEscape == false)
                {
                    if (Arguments.Substring(index + 1).StartsWith(aliasLabel, StringComparison.InvariantCultureIgnoreCase))
                    {
                        hasAlias = true;
                        aliasStartIndex = index + aliasLabel.Length + 1;
                    }
                    break;
                }
                sb.Append(c);
            }

            if (hasAlias)
            {
                int closeParenIndex = Arguments.IndexOf(")", aliasStartIndex, StringComparison.InvariantCultureIgnoreCase);
                string aliasName = Arguments.Substring(aliasStartIndex, closeParenIndex - aliasStartIndex);
                sb.Append($" ({aliasName})");
            }

            return sb.ToString();
        }
    }
}
