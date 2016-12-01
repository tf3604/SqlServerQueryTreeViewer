using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerParseTreeViewer
{
    internal static class TreeNodeIconMapper
    {
        public static Icon GetIconForNode(SqlParseTreeNode node)
        {
            switch (node.Operation)
            {
                case OperationType.LogOp_Get:
                    return SqlServerParseTreeViewerResources.GetIcon;

                default:
                    return null;
            }
        }
    }
}
