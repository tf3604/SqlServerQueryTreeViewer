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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    public partial class ParseTreeTab : UserControl
    {
        private static int _targetLineLength = 30;

        private List<NodeIcon> _icons;
        private ToolTip _currentToolTip;
        private NodeIcon _currentNodeIcon = null;
        private Point _toolTipMouseLocation;

        public ParseTreeTab()
        {
            InitializeComponent();
            _icons = new List<NodeIcon>();
            DrawingSurface.MouseWheel += DrawingSurface_MouseWheel;
        }

        private void DrawingSurface_MouseWheel(object sender, MouseEventArgs e)
        {
            return;
        }

        public PictureBox DrawingSurface
        {
            get
            {
                return this.treeViewImage;
            }
        }

        public string TreeText
        {
            get
            {
                return this.treeTextBox.Text;
            }
            set
            {
                this.treeTextBox.Text = value;
            }
        }

        public TabControl TreeTabControl
        {
            get
            {
                return treeTab;
            }
        }

        public bool IsMemo
        {
            set
            {
                if (value == true)
                {
                    treeViewTab.Text = "Memo View";
                    treeTextTab.Text = "Memo Text";
                }
                else
                {
                    treeViewTab.Text = "Tree View";
                    treeTextTab.Text = "Tree Text";
                }
            }
        }

        public void SetIcons(List<NodeIcon> icons)
        {
            if (icons == null)
            {
                throw new ArgumentNullException(nameof(icons));
            }

            _icons.Clear();
            _icons.AddRange(icons);
        }

        private NodeIcon GetMatchingNodeIcon(Point position)
        {
            foreach (NodeIcon icon in _icons)
            {
                if (position.X >= icon.Left &&
                    position.X <= icon.Left + icon.Width &&
                    position.Y >= icon.Top &&
                    position.Y <= icon.Top + icon.Height)
                {
                    return icon;
                }
            }

            return null;
        }

        private void TreeViewImage_MouseClick(object sender, MouseEventArgs e)
        {
            string noArgumentsMessage = "[No arguments to display]";
            Point mouseLocation = new Point(e.X, e.Y);
            NodeIcon selectedIcon = GetMatchingNodeIcon(mouseLocation);
            if (selectedIcon != null)
            {
                if (_currentToolTip != null)
                {
                    _currentToolTip.Hide(treeViewImage);
                    _currentToolTip.Dispose();
                    _currentToolTip = null;
                }

                string text = string.Empty;
                if (selectedIcon is TreeNodeIcon)
                {
                    TreeNodeIcon treeIcon = selectedIcon as TreeNodeIcon;
                    text = FormatText(treeIcon.Node.Arguments);
                    if (string.IsNullOrEmpty(text))
                    {
                        text = noArgumentsMessage;
                    }

                    text = treeIcon.Node.OperationName + Environment.NewLine + text;
                }
                else if (selectedIcon is MemoNodeIcon)
                {
                    MemoNodeIcon memoIcon = selectedIcon as MemoNodeIcon;
                    text = FormatText(memoIcon.Node.Arguments);
                    if (string.IsNullOrEmpty(text))
                    {
                        text = noArgumentsMessage;
                    }

                    text = memoIcon.Node.OperationName + Environment.NewLine + text;
                }

                text = text.Trim();

                int numberOfLines = text.Count(c => c == '\r') + 1;

                _currentToolTip = new ToolTip();
                _currentToolTip.IsBalloon = true;
                _currentToolTip.Show(
                    text, 
                    treeViewImage, 
                    selectedIcon.Left + selectedIcon.Width - 25, 
                    selectedIcon.Top - 44 - 13 * numberOfLines);
                _toolTipMouseLocation = mouseLocation;
                _currentNodeIcon = selectedIcon;
            }
        }

        private void TreeViewImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentToolTip != null)
            {
                Point mouseLocation = new Point(e.X, e.Y);
                NodeIcon selectedIcon = GetMatchingNodeIcon(mouseLocation);
                if (selectedIcon == null ||
                    selectedIcon != _currentNodeIcon)
                {
                    _currentToolTip.Hide(treeViewImage);
                    _currentToolTip.Dispose();
                    _currentToolTip = null;
                }
            }

            // Check for mouse wheel movement
            if (e.Delta != 0)
            {

            }
        }

        private static string FormatText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            int whitespaceIndex;
            while (text.Length > _targetLineLength &&
                (whitespaceIndex = text.IndexOf(' ', _targetLineLength)) >= 0)
            {
                string left = text.Substring(0, whitespaceIndex);
                text = text.Substring(whitespaceIndex + 1);
                sb.AppendLine(left);
            }

            if (string.IsNullOrEmpty(text) == false)
            {
                sb.Append(text);
            }

            return sb.ToString();
        }
    }
}
