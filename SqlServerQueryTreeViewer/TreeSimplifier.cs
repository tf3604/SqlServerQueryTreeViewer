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
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    public static class TreeSimplifier
    {
        private static List<OperationType> x = new List<OperationType>()
        {
            OperationType.AncOp_PrjList
        };

        private static Dictionary<OperationType, object> lowValueNodeOperations = new Dictionary<OperationType, object>()
        {
            { OperationType.AncOp_PrjList , null }
        };

        public static SqlParseTree RemoveLowValueLeafLevelNodes(SqlParseTree inputTree)
        {
            SqlParseTree tree = SqlParseTree.Clone(inputTree);
            RemoveLowValueLeafLevelNodes(tree.RootNode);
            return tree;
        }

        private static void RemoveLowValueLeafLevelNodes(SqlParseTreeNode parentNode)
        {
            List<SqlParseTreeNode> nodesToRemove = null;
            foreach (SqlParseTreeNode childNode in parentNode.Children)
            {
                if (string.IsNullOrEmpty(childNode.Arguments) &&
                    childNode.Children.Count == 0 &&
                    lowValueNodeOperations.ContainsKey(childNode.Operation))
                {
                    if (nodesToRemove == null)
                    {
                        nodesToRemove = new List<SqlParseTreeNode>();
                    }

                    nodesToRemove.Add(childNode);
                }
            }

            if (nodesToRemove != null)
            {
                nodesToRemove.ForEach(n => parentNode.Children.Remove(n));
            }

            parentNode.Children.ForEach(n => RemoveLowValueLeafLevelNodes(n));
        }
    }
}
