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
    public static class TreeVisualizer
    {
        private const int _blockWidth = 100;
        private const int _blockHeight = 30;
        private const int _spacingWidth = 30;
        private const int _spacingHeight = 80;
        private const int _interChildSpacingWidth = 30;
        private const int _leftMargin = 110;
        private const int _rightMargin = 110;
        private const int _topMargin = 60;
        private const int _bottomMargin = 60;

        //private static StringBuilder _operatorList = new StringBuilder();
        //private static List<OperationType> _operators = new List<OperationType>();

        public static Bitmap Render(SqlParseTree tree, out List<NodeIcon> nodeIcons)
        {
            if (tree == null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            int maxDepth;
            int maxWidth;
            List<NodeIcon> icons = FlattenTree(tree, out maxDepth, out maxWidth);

            int width = _leftMargin + _rightMargin + maxWidth * _blockWidth + (maxWidth - 1) * _spacingWidth;
            int height = _topMargin + _bottomMargin + maxDepth * _blockHeight + (maxDepth - 1) * _spacingHeight;
            CenterLevels(icons, width);
            BalanceNodes(icons);
            PositionNodes(icons, out width);

            Font font = new Font("Times New Roman", 10);
            Pen leafNodeBorderPen = new Pen(Color.Red, 2);

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                foreach (NodeIcon icon in icons)
                {
                    Rectangle rectangle = new Rectangle(icon.Left, icon.Top, icon.Width, icon.Height);
                    //graphics.FillRectangle(Brushes.LightSteelBlue, rectangle);
                    Brush brush = GetBrushForOperator(icon.Node.OperationName);
                    Pen borderPen = icon.IsLeafNode ? leafNodeBorderPen : null;
                    DrawFilledRoundedRectangle(graphics, brush, rectangle, borderPen);
                    SizeF textSize = graphics.MeasureString(icon.Node.OperationName, font);
                    RectangleF textRectangle = new RectangleF(
                        icon.Left + icon.Width / 2 - textSize.Width / 2,
                        icon.Top + icon.Height / 2 - textSize.Height / 2,
                        textSize.Width,
                        textSize.Height);
                    graphics.DrawString(icon.Node.OperationName, font, Brushes.Black, textRectangle);

                    if (icon.Parent != null)
                    {
                        graphics.DrawLine(Pens.Black, icon.Left + icon.Width / 2, icon.Top, icon.Parent.Left + icon.Parent.Width / 2, icon.Parent.Top + icon.Parent.Height);
                    }
                }
            }

            nodeIcons = icons;
            ExtractOperators(tree);
            return bitmap;
        }

        private static void DrawFilledRoundedRectangle(Graphics graphics, Brush brush, Rectangle rectangle, Pen optionalBorderPen)
        {
            int radius = 5;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            path.AddArc(rectangle.Left, rectangle.Top, 2 * radius, 2 * radius, 180, 90);
            path.AddLine(rectangle.Left + radius, rectangle.Top, rectangle.Left + rectangle.Width - radius, rectangle.Top);
            path.AddArc(rectangle.Left + rectangle.Width - 2 * radius, rectangle.Top, 2 * radius, 2 * radius, 270, 90);
            path.AddLine(rectangle.Left + rectangle.Width, rectangle.Top + radius, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height - radius);
            path.AddArc(rectangle.Left + rectangle.Width - 2 * radius, rectangle.Top + rectangle.Height - 2 * radius, 2 * radius, 2 * radius, 0, 90);
            path.AddLine(rectangle.Left + rectangle.Width - radius, rectangle.Top + rectangle.Height, rectangle.Left + radius, rectangle.Top + rectangle.Height);
            path.AddArc(rectangle.Left, rectangle.Top + rectangle.Height - 2 * radius, 2 * radius, 2 * radius, 90, 90);
            path.AddLine(rectangle.Left, rectangle.Top + rectangle.Height - radius, rectangle.Left, rectangle.Top + radius);

            path.CloseFigure();

            graphics.FillPath(brush, path);
            if (optionalBorderPen != null)
            {
                graphics.DrawPath(optionalBorderPen, path);
            }
        }

        private static List<NodeIcon> FlattenTree(SqlParseTree tree, out int maxDepth, out int maxWidth)
        {
            Dictionary<int, int> levelCounts = new Dictionary<int, int>();
            List<NodeIcon> icons = new List<NodeIcon>();

            MeasureNode(tree.RootNode, 0, levelCounts, icons);

            maxDepth = levelCounts.Keys.Count;
            maxWidth = levelCounts.Values.Max();
            FixupParentLinks(icons);

            return icons;
        }

        private static void MeasureNode(SqlParseTreeNode node, int depth, Dictionary<int, int> levelCounts, List<NodeIcon> icons)
        {
            if (levelCounts.ContainsKey(depth) == false)
            {
                levelCounts.Add(depth, 0);
            }
            levelCounts[depth]++;

            NodeIcon icon = new NodeIcon();

            icon.Node = node;
            icon.X = levelCounts[depth] - 1;
            icon.Y = depth;
            icon.Left = _leftMargin + icon.X * (_blockWidth + _spacingWidth);
            icon.Top = _topMargin + icon.Y * (_blockHeight + _spacingHeight);
            icon.Width = _blockWidth;
            icon.Height = _blockHeight;

            icons.Add(icon);

            foreach (SqlParseTreeNode child in node.Children)
            {
                MeasureNode(child, depth + 1, levelCounts, icons);
            }
        }

        private static void FixupParentLinks(List<NodeIcon> icons)
        {
            Dictionary<SqlParseTreeNode, NodeIcon> iconMap = new Dictionary<SqlParseTreeNode, NodeIcon>();
            icons.ForEach(i => iconMap.Add(i.Node, i));

            foreach (NodeIcon icon in icons)
            {
                SqlParseTreeNode parentNode = icon.Node.Parent;
                if (parentNode != null)
                {
                    icon.Parent = iconMap[parentNode];
                }

                foreach (SqlParseTreeNode childNode in icon.Node.Children)
                {
                    icon.Children.Add(iconMap[childNode]);
                }
            }
        }

        private static void CenterLevels(List<NodeIcon> icons, int overallWidth)
        {
            Dictionary<int, List<NodeIcon>> iconsByLevel = new Dictionary<int, List<NodeIcon>>();
            foreach (NodeIcon icon in icons)
            {
                if (iconsByLevel.ContainsKey(icon.Y) == false)
                {
                    iconsByLevel.Add(icon.Y, new List<NodeIcon>());
                }
                iconsByLevel[icon.Y].Add(icon);
            }

            foreach (int level in iconsByLevel.Keys)
            {
                int left = iconsByLevel[level].Min(i => i.Left);
                int right = iconsByLevel[level].Max(i => i.Left + i.Width);
                int center = (left + right) / 2;
                int shift = overallWidth / 2 - center;

                iconsByLevel[level].ForEach(i => i.Left += shift);
            }
        }

        private static void BalanceNodes(List<NodeIcon> icons)
        {
            int depth = icons.Max(i => i.Y);
            while (depth >= 0)
            {
                List<NodeIcon> iconsAtDepth = icons.Where(i => i.Y == depth).ToList();
                iconsAtDepth.Sort((a, b) => a.X.CompareTo(b.X));

                foreach (NodeIcon icon in iconsAtDepth)
                {
                    icon.DescendantWidth =
                        Math.Max(
                            _blockWidth,
                            icon.Children.Sum(c => c.DescendantWidth) + _interChildSpacingWidth * (icon.Children.Count - 1));
                }

                depth--;
            }
        }

        private static void PositionNodes(List<NodeIcon> icons, out int width)
        {
            // Get the root node
            NodeIcon root = icons.FirstOrDefault(i => i.Parent == null);
            if (root == null)
            {
                throw new ApplicationException("Cannot find root node!");
            }

            width = _leftMargin + root.DescendantWidth + _rightMargin;
            root.Top = _topMargin;
            root.Left = width / 2 - _blockWidth / 2;
            int level = 0;

            List<NodeIcon> nodes = new List<NodeIcon>() { root };
            while (nodes.Count > 0)
            {
                level++;
                foreach (NodeIcon node in nodes)
                {
                    node.Children.Sort((a, b) => a.X.CompareTo(b.X));
                    foreach (NodeIcon child in node.Children)
                    {
                        int previousSiblingWidth = node.Children.Where(n => n.X < child.X).Sum(n => n.DescendantWidth) +
                            node.Children.Count(n => n.X < child.X) * _interChildSpacingWidth;
                        double relativePosition = 1.0 * (previousSiblingWidth + child.DescendantWidth / 2) / node.DescendantWidth;

                        child.Top = _topMargin + (_blockHeight + _spacingHeight) * level;
                        child.Left = (int)(node.Left - node.DescendantWidth / 2 + relativePosition * node.DescendantWidth);
                    }
                }

                nodes = nodes.SelectMany(n => n.Children).ToList();
            }
        }

        private static Brush GetBrushForOperator(string operatorName)
        {
            return new SolidBrush(GetColorForOperator(operatorName));
        }

        private static Color GetColorForOperator(string operatorName)
        {
            if (string.IsNullOrEmpty(operatorName) == false)
            {
                Color? color = ViewerSettings.Instance.GetOperatorColor(operatorName);
                if (color != null)
                {
                    return color.Value;
                }
            }

            switch (operatorName)
            {
                case "LogOp_Project":
                    return Color.LightGreen;

                case "LogOp_Get":
                    return Color.LightBlue;

                case "LogOp_Select":
                    return Color.CornflowerBlue;

                case "ScaOp_Const":
                    return Color.LightSalmon;

                case "ScaOp_Identifier":
                    return Color.LightPink;

                case "ScaOp_Logical":
                    return Color.LightGoldenrodYellow;

                case "ScaOp_Comp":
                    return Color.Goldenrod;

                case "AncOp_PrjList":
                    return Color.LightGreen;

                default:
                    return Color.LightSteelBlue;
            }
        }

        private static void ExtractOperators(SqlParseTree tree)
        {
            List<OperatorColor> operatorColors = new List<OperatorColor>(ViewerSettings.Instance.OperatorColors);
            Dictionary<string, OperatorColor> index = new Dictionary<string, OperatorColor>();
            operatorColors.ForEach(o => { if (index.ContainsKey(o.OperatorName) == false) { index.Add(o.OperatorName, o); } });
            List<SqlParseTreeNode> nodes = new List<SqlParseTreeNode>() { tree.RootNode };
            while (nodes.Count > 0)
            {
                foreach (SqlParseTreeNode node in nodes)
                {
                    if (string.IsNullOrEmpty(node.OperationName) == false &&
                        index.ContainsKey(node.OperationName) == false)
                    {
                        OperatorColor operatorColor = new OperatorColor();
                        operatorColor.OperatorName = node.OperationName;
                        operatorColor.DisplayColor = GetColorForOperator(node.OperationName);
                        index.Add(node.OperationName, operatorColor);
                        ViewerSettings.Instance.AddOperatorColor(node.OperationName, operatorColor.DisplayColor);
                    }
                }

                nodes = nodes.SelectMany(n => n.Children).ToList();
            }
        }
    }
}
