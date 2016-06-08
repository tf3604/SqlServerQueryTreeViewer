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
