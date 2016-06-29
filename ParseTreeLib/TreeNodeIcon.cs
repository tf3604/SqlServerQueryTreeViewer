using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class TreeNodeIcon : NodeIcon
    {
        public TreeNodeIcon()
        {
            this.Children = new List<TreeNodeIcon>();
        }

        public SqlParseTreeNode Node
        {
            get;
            set;
        }

        public TreeNodeIcon Parent
        {
            get;
            set;
        }

        public List<TreeNodeIcon> Children
        {
            get;
            set;
        }

        public bool IsLeafNode
        {
            get
            {
                return this.Node.Children.Count == 0;
            }
        }

        public int DescendantWidth
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
