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

namespace SqlServerParseTreeViewer
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

                string text = FormatText(selectedIcon.Node.Arguments);
                if (string.IsNullOrEmpty(text))
                {
                    text = "[No arguments to display]";
                }

                text = selectedIcon.Node.OperationName + Environment.NewLine + text;
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
