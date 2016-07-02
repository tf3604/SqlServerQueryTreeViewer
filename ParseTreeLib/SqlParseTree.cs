//  Copyright(c) 2016 Brian Hansen.

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
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlParseTree
    {
        public SqlParseTree()
        {
        }

        public string TreeDescription
        {
            get;
            set;
        }

        public SqlParseTreeNode RootNode
        {
            get;
            set;
        }

        public string OuterTreeText
        {
            get;
            set;
        }

        public string InnerTreeText
        {
            get;
            set;
        }

        public int BeginOffset
        {
            get;
            set;
        }

        public int EndOffset
        {
            get;
            set;
        }

        public void MeasureTree(out int maxDepth, out int maxWidth)
        {
            Dictionary<int, int> levelCounts = new Dictionary<int, int>();
            MeasureNode(this.RootNode, 0, levelCounts);

            maxDepth = levelCounts.Keys.Max();
            maxWidth = levelCounts.Values.Max();
        }

        private static void MeasureNode(SqlParseTreeNode node, int depth, Dictionary<int, int> levelCounts)
        {
            if (levelCounts.ContainsKey(depth) == false)
            {
                levelCounts.Add(depth, 0);
            }
            levelCounts[depth]++;

            foreach (SqlParseTreeNode child in node.Children)
            {
                MeasureNode(child, depth + 1, levelCounts);
            }
        }
    }
}
