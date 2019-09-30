//  Copyright(c) 2016-2019 Brian Hansen.

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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    internal static class TreeNodeIconMapper
    {
        public static Icon GetIconForNode(SqlParseTreeNode node)
        {
            switch (node.Operation)
            {
                case OperationType.LogOp_Get:
                    return SqlServerQueryTreeViewerResources.Get;

                case OperationType.LogOp_Join:
                    return SqlServerQueryTreeViewerResources.InnerJoin;

                case OperationType.LogOp_LeftOuterJoin:
                    return SqlServerQueryTreeViewerResources.LeftOuterJoin;

                case OperationType.LogOp_RightOuterJoin:
                    return SqlServerQueryTreeViewerResources.RightOuterJoin;

                case OperationType.LogOp_LeftSemiJoin:
                    return SqlServerQueryTreeViewerResources.LeftSemiJoin;

                case OperationType.LogOp_LeftAntiSemiJoin:
                    return SqlServerQueryTreeViewerResources.LeftAntiSemiJoin;

                case OperationType.LogOp_RightSemiJoin:
                    return SqlServerQueryTreeViewerResources.RightSemiJoin;

                case OperationType.LogOp_RightAntiSemiJoin:
                    return SqlServerQueryTreeViewerResources.RightAntiSemiJoin;

                case OperationType.LogOp_Project:
                    return SqlServerQueryTreeViewerResources.Project;

                case OperationType.ScaOp_Comp:
                    switch (node.Arguments)
                    {
                        case "x_cmpEq":
                            return SqlServerQueryTreeViewerResources.CompareEqual;

                        default:
                            return null;
                    }

                case OperationType.ScaOp_Const:
                    return SqlServerQueryTreeViewerResources.Const;

                case OperationType.ScaOp_Identifier:
                    return SqlServerQueryTreeViewerResources.Identifier;

                default:
                    return null;
            }
        }

        public static string ReportUnmappedOperators()
        {
            StringBuilder sb = new StringBuilder();
            List<OperatorColor> operatorColors = ViewerSettings.Instance.OperatorColors.ToList();
            foreach (OperatorColor operatorColor in operatorColors)
            {
                string operatorName = operatorColor.OperatorName;
                OperationType operationType;
                if (Enum.TryParse<OperationType>(operatorName, out operationType))
                {
                    SqlParseTreeNode node = new SqlParseTreeNode();
                    node.Operation = operationType;
                    Icon icon = GetIconForNode(node);
                    if (icon == null)
                    {
                        sb.AppendFormat("Unmapped operator {0}", operatorName);
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendFormat("Operator has no enum type: {0}", operatorName);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
