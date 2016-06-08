using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class NodeIcon
    {
        public NodeIcon()
        {
            this.Children = new List<NodeIcon>();
        }

        public SqlParseTreeNode Node
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int Left
        {
            get;
            set;
        }

        public int Top
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public NodeIcon Parent
        {
            get;
            set;
        }

        public List<NodeIcon> Children
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
