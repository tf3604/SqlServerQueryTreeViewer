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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerParseTreeViewer
{
    public class PlanStyleHorizontalTreeVisualizer : TreeVisualizer
    {
        private const int _iconWidth = 32;
        private const int _iconHeight = 32;
        private const int _interIconHorizontalSpacing = 50;
        private const int _interIconVerticalSpacing = 30;
        private const int _iconToTextSpacing = 5;
        private const int _connectorHorizontalSpacing = 10;
        private const int _connectorVerticalSpacing = 5;
        private const int _leftMargin = 5;
        private const int _rightMargin = 5;
        private const int _topMargin = 5;
        private const int _bottomMargin = 5;

        private Graphics _dummyGraphics;
        private Font _textFont = new Font("Times New Roman", 10);

        public PlanStyleHorizontalTreeVisualizer()
        {
        }

        public override Bitmap Render(SqlParseTree tree, out List<TreeNodeIcon> nodeIcons)
        {
            if (tree == null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            using (Bitmap dummyBitmap = new Bitmap(1, 1))
            {
                _dummyGraphics = Graphics.FromImage(dummyBitmap);
                try
                {

                    nodeIcons = PositionNodes(tree);
                }
                finally
                {
                    _dummyGraphics.Dispose();
                }
            }

            int width = _leftMargin + nodeIcons.GetWidth() + _rightMargin;
            int height = _topMargin + nodeIcons.GetHeight() + _bottomMargin;

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                foreach (TreeNodeIcon icon in nodeIcons)
                {
                    if (icon.Icon != null)
                    {
                        graphics.DrawIcon(icon.Icon, icon.IconRectangle);
                    }
                    else
                    {
                        graphics.DrawRectangle(Pens.Black, icon.IconRectangle);
                    }
                    graphics.DrawString(icon.Text, _textFont, Brushes.Black, icon.TextRectangle);

                    // Draw connectors between icons
                    if (icon.Children.Count > 0)
                    {
                        int arrowLeft = icon.IconRectangle.Left + icon.IconRectangle.Width + _connectorHorizontalSpacing;
                        int firstArrowTop = (icon.IconRectangle.Top + icon.IconRectangle.Bottom) / 2 - _connectorVerticalSpacing * (icon.Children.Count - 1) / 2;
                        int arrowTop = firstArrowTop;

                        if (arrowTop + _connectorVerticalSpacing * (icon.Children.Count - 1) > icon.IconRectangle.Bottom)
                        {
                            arrowLeft = Math.Max(arrowLeft, icon.TextRectangle.Left + icon.TextRectangle.Width + _connectorHorizontalSpacing);
                        }

                        int centerAdjustment = _connectorHorizontalSpacing * icon.Children.Count / 2;
                        bool isFirst = true;

                        foreach (var childIcon in icon.Children)
                        {
                            int arrowRight = childIcon.IconRectangle.Left - _connectorHorizontalSpacing;
                            int arrowBottom = isFirst ? firstArrowTop : (childIcon.IconRectangle.Top + childIcon.IconRectangle.Bottom) / 2;
                            int arrowHorizontalCenter = (arrowLeft + arrowRight) / 2 + centerAdjustment;

                            graphics.DrawLine(Pens.Black, arrowLeft, arrowTop, arrowHorizontalCenter, arrowTop);
                            graphics.DrawLine(Pens.Black, arrowHorizontalCenter, arrowTop, arrowHorizontalCenter, arrowBottom);
                            graphics.DrawLine(Pens.Black, arrowHorizontalCenter, arrowBottom, arrowRight, arrowBottom);

                            isFirst = false;
                            arrowTop += _connectorVerticalSpacing;
                            centerAdjustment -= _connectorHorizontalSpacing;
                        }
                    }
                }
            }

            return bitmap;
        }

        private List<TreeNodeIcon> PositionNodes(SqlParseTree tree)
        {
            List<TreeNodeIcon> nodeIcons = PositionSubtree(tree.RootNode, _leftMargin, _topMargin);
            return nodeIcons;
        }

        private List<TreeNodeIcon> PositionSubtree(SqlParseTreeNode head, int top, int left)
        {
            List<TreeNodeIcon> icons = new List<TreeNodeIcon>();

            int originalTop = top;
            //int adjust = _connectorVerticalSpacing * head.Children.Count / 2 - _interIconVerticalSpacing;
            int firstArrowTop = (2 * top + _iconHeight) / 2 - _connectorVerticalSpacing * (head.Children.Count - 1) / 2;
            if (top > firstArrowTop)
            {
                top += (top - firstArrowTop) + _iconHeight / 2;
            }

            TreeNodeIcon icon = PositionNode(head, top, left);
            icons.Add(icon);

            int horizontalSpacing = Math.Max(_interIconHorizontalSpacing, _connectorHorizontalSpacing * head.Children.Count);
            int newLeft = left + icon.Width + horizontalSpacing;
            //int newTop = originalTop;
            int altTop = (icon.IconRectangle.Top + icon.IconRectangle.Bottom) / 2 - _connectorVerticalSpacing * (icon.Children.Count - 1) / 2;
            int newTop = Math.Min(originalTop, altTop);

            foreach (SqlParseTreeNode child in head.Children)
            {
                List<TreeNodeIcon> childIcons = PositionSubtree(child, newTop, newLeft);
                if (childIcons.Count > 0)
                {
                    childIcons[0].Parent = icon;
                    icon.Children.Add(childIcons[0]);
                }
                icons.AddRange(childIcons);
                newTop += childIcons.GetHeight() + _interIconVerticalSpacing;
            }

            return icons;
        }

        private TreeNodeIcon PositionNode(SqlParseTreeNode node, int top, int left)
        {
            string nodeText = node.ToString();
            SizeF textSize = _dummyGraphics.MeasureString(nodeText, _textFont);

            TreeNodeIcon icon = new TreeNodeIcon();
            icon.X = left;
            icon.Y = top;
            icon.Width = (int)Math.Max(_iconWidth, textSize.Width);
            icon.Height = _iconHeight + _iconToTextSpacing + (int)textSize.Height;
            icon.Node = node;
            icon.Text = nodeText;
            icon.TextRectangle = new Rectangle(left + (icon.Width - (int)textSize.Width) / 2, top + _iconHeight + _iconToTextSpacing, (int)textSize.Width + 2, (int)textSize.Height + 2);
            icon.IconRectangle = new Rectangle(left + (icon.Width - _iconWidth) / 2, top, _iconWidth, _iconHeight);
            icon.Icon = TreeNodeIconMapper.GetIconForNode(node);

            return icon;
        }
    }
}
