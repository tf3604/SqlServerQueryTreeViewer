﻿//  Copyright(c) 2016 Brian Hansen.

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

        public string Text
        {
            get;
            set;
        }

        public Rectangle TextRectangle
        {
            get;
            set;
        }

        public Icon Icon
        {
            get;
            set;
        }

        public Rectangle IconRectangle
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
