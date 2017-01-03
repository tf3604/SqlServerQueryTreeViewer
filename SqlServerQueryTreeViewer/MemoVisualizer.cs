//  Copyright(c) 2016-2017 Brian Hansen.

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
using static System.Math;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    public static class MemoVisualizer
    {
        private static Font _font = null;
        private static int _cellBufferHeight = 5;
        private static int _cellBufferWidth = 5;
        private static int _headerColumnWidth = 75;
        private static int _headerRowHeight = 20;

        public static Bitmap Render(SqlMemo memo, out List<MemoNodeIcon> memoNodeIcons)
        {
            if (memo == null)
            {
                throw new ArgumentNullException(nameof(memo));
            }

            if (_font == null)
            {
                _font = new Font("Times New Roman", 10);
            }

            int columnCount;
            int rowCount;
            Dictionary<int, float> columnWidths;
            Dictionary<int, float> rowHeights;

            // Create a temporary bitmap so we can compute some size information
            Bitmap bitmap = new Bitmap(1, 1);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                GetMemoExtent(memo, graphics, out columnCount, out rowCount, out columnWidths, out rowHeights);
            }

            int overallWidth = columnWidths.Sum(c => (int)Ceiling(c.Value)) + _headerColumnWidth + 1;
            int overallHeight = rowHeights.Sum(r => (int)Ceiling(r.Value)) + _headerRowHeight + 1;

            Rectangle[,] cells = new Rectangle[columnCount, rowCount];

            bitmap = new Bitmap(overallWidth, overallHeight);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, overallWidth, overallHeight);
                Pen gridlinePen = Pens.Black;
                graphics.DrawLine(gridlinePen, 0, 0, overallWidth, 0);
                // Draw the columns
                int columnPosition = _headerColumnWidth;
                for (int columnIndex = 0; columnIndex <= columnCount; columnIndex++)
                {
                    graphics.DrawLine(gridlinePen, columnPosition, 0, columnPosition, overallHeight);
                    if (columnIndex < columnCount)
                    {
                        int columnWidth = columnWidths.ContainsKey(columnIndex) ? (int)Ceiling(columnWidths[columnIndex]) : 0;
                        columnPosition += columnWidth;
                    }
                }

                graphics.DrawLine(gridlinePen, 0, 0, 0, overallHeight);
                // Draw the rows
                int rowPosition = _headerRowHeight;
                for (int rowIndex = 0; rowIndex <= rowCount; rowIndex++)
                {
                    graphics.DrawLine(gridlinePen, 0, rowPosition, overallWidth, rowPosition);
                    if (rowIndex < rowCount)
                    {
                        int rowHeight = rowHeights.ContainsKey(rowIndex) ? (int)Ceiling(rowHeights[rowIndex]) : 0;
                        rowPosition += rowHeight;

                        columnPosition = _headerColumnWidth;
                        for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                        {
                            int columnWidth = columnWidths.ContainsKey(columnIndex) ? (int)Ceiling(columnWidths[columnIndex]) : 0;
                            columnPosition += columnWidth;
                            cells[columnIndex, rowCount - 1 - rowIndex] = new Rectangle(columnPosition - columnWidth, rowPosition - rowHeight, columnWidth, rowHeight);
                        }
                    }
                }

                Brush operationNameBrush = Brushes.Black;
                Brush nodeIdentifierBrush = Brushes.Red;
                Brush headerBrush = Brushes.LightSkyBlue;
                Brush rootBrush = Brushes.LightGreen;

                // Draw the header row
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    string cellName = "x." + columnIndex.ToString();
                    SizeF size = graphics.MeasureString(cellName, _font);
                    RectangleF containingRectangle = new RectangleF(cells[columnIndex, 0].Left + 1, 1, cells[columnIndex, 0].Width - 1, _headerRowHeight - 1);
                    graphics.FillRectangle(headerBrush, containingRectangle);
                    PointF stringStartingPoint = new PointF(
                        containingRectangle.Left + containingRectangle.Width / 2 - size.Width / 2,
                        containingRectangle.Top + containingRectangle.Height / 2 - size.Height / 2);
                    graphics.DrawString(cellName, _font, nodeIdentifierBrush, stringStartingPoint);
                }

                // Draw the header column
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    string cellName = rowIndex.ToString() + ".x";
                    SizeF size = graphics.MeasureString(cellName, _font);
                    RectangleF containingRectangle = new RectangleF(1, cells[0, rowIndex].Top + 1, _headerColumnWidth - 1, cells[0, rowIndex].Height - 1);
                    graphics.FillRectangle(headerBrush, containingRectangle);
                    PointF stringStartingPoint = new PointF(
                        containingRectangle.Left + containingRectangle.Width / 2 - size.Width / 2,
                        containingRectangle.Top + containingRectangle.Height / 2 - size.Height / 2);
                    graphics.DrawString(cellName, _font, nodeIdentifierBrush, stringStartingPoint);
                }

                // Fill the upper left cell
                graphics.FillRectangle(headerBrush, 1, 1, _headerColumnWidth - 1, _headerRowHeight - 1);

                // Fill the root row.  Should be only one, but we'll handle if that's not the case.
                foreach (SqlMemoGroup rootGroup in memo.Groups.Where(g => g.IsRoot))
                {
                    int rootRowIndex = rootGroup.GroupNumber;
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        Rectangle outerRectangle = cells[columnIndex, rootRowIndex];
                        Rectangle containingRectangle = new Rectangle(outerRectangle.Left + 1, outerRectangle.Top + 1, outerRectangle.Width - 1, outerRectangle.Height - 1);
                        graphics.FillRectangle(rootBrush, containingRectangle);
                    }
                }

                memoNodeIcons = new List<MemoNodeIcon>();
                Dictionary<SqlMemoNodeIdentifier, MemoNodeIcon> nodeIndex = new Dictionary<SqlMemoNodeIdentifier, MemoNodeIcon>();

                // Draw the nodes
                foreach (SqlMemoNode node in memo.Groups.SelectMany(g => g.Operations))
                {
                    int rowIndex = node.Identifier.GroupNumber;
                    int columnIndex = node.Identifier.OperationNumber;
                    SizeF fullSize = graphics.MeasureString(node.DisplayName, _font);
                    SizeF operationSize = graphics.MeasureString(node.DisplayOperationName, _font);
                    SizeF parentSize = graphics.MeasureString(node.DisplayParentName, _font);
                    Rectangle outerRectangle = cells[columnIndex, rowIndex];
                    RectangleF containingRectangle = new RectangleF(outerRectangle.Left + 1, outerRectangle.Top + 1, outerRectangle.Width - 1, outerRectangle.Height - 1);
                    PointF stringStartingPoint = new PointF(
                        containingRectangle.Left + containingRectangle.Width / 2 - fullSize.Width / 2,
                        containingRectangle.Top + containingRectangle.Height / 2 - fullSize.Height / 2);
                    PointF parentStartingPoint = new PointF(
                        stringStartingPoint.X + operationSize.Width,
                        stringStartingPoint.Y);
                    graphics.DrawString(node.DisplayOperationName, _font, operationNameBrush, stringStartingPoint);
                    graphics.DrawString(node.DisplayParentName, _font, nodeIdentifierBrush, parentStartingPoint);

                    MemoNodeIcon icon = new MemoNodeIcon();
                    icon.Node = node;
                    icon.X = columnIndex;
                    icon.Y = rowIndex;
                    icon.Top = (int)containingRectangle.Top;
                    icon.Left = (int)containingRectangle.Left;
                    icon.Height = (int)containingRectangle.Height;
                    icon.Width = (int)containingRectangle.Width;

                    memoNodeIcons.Add(icon);

                    if (nodeIndex.ContainsKey(node.Identifier) == false)
                    {
                        nodeIndex.Add(node.Identifier, icon);
                    }
                }

                // Fixup parent links
                foreach (MemoNodeIcon icon in memoNodeIcons)
                {
                    foreach (SqlMemoNode parentNode in icon.Node.Parents)
                    {
                        if (nodeIndex.ContainsKey(parentNode.Identifier))
                        {
                            MemoNodeIcon parentIcon = nodeIndex[parentNode.Identifier];
                            icon.Parents.Add(parentIcon);
                        }
                    }
                }
            }

            return bitmap;
        }

        private static void GetMemoExtent(
            SqlMemo memo, 
            Graphics graphics, 
            out int columnCount, 
            out int rowCount, 
            out Dictionary<int, float> columnWidths, 
            out Dictionary<int, float> rowHeights)
        {
            columnCount = 0;
            rowCount = 0;
            columnWidths = new Dictionary<int, float>();
            rowHeights = new Dictionary<int, float>();

            foreach (SqlMemoNode node in memo.Groups.SelectMany(n => n.Operations))
            {
                if (node.Identifier.GroupNumber >= rowCount)
                {
                    rowCount = node.Identifier.GroupNumber + 1;
                }
                if (node.Identifier.OperationNumber >= columnCount)
                {
                    columnCount = node.Identifier.OperationNumber + 1;
                }

                int columnId = node.Identifier.OperationNumber;
                if (columnWidths.ContainsKey(columnId) == false)
                {
                    columnWidths.Add(columnId, 0);
                }

                int rowId = node.Identifier.GroupNumber;
                if (rowHeights.ContainsKey(rowId) == false)
                {
                    rowHeights.Add(rowId, 0);
                }

                SizeF size = graphics.MeasureString(node.DisplayName, _font);
                if (size.Width + 2 * _cellBufferWidth > columnWidths[columnId])
                {
                    columnWidths[columnId] = size.Width + 2 * _cellBufferWidth;
                }
                if (size.Height + 2 * _cellBufferHeight > rowHeights[rowId])
                {
                    rowHeights[rowId] = size.Height + 2 * _cellBufferHeight;
                }
            }

            // Add missed columns and set minimums
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                if (columnWidths.ContainsKey(columnIndex) == false)
                {
                    columnWidths.Add(columnIndex, _headerColumnWidth);
                }
                else
                {
                    columnWidths[columnIndex] = Max(columnWidths[columnIndex], _headerColumnWidth);
                }
            }

            // Add missed rows and set minimums
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (rowHeights.ContainsKey(rowIndex) == false)
                {
                    rowHeights.Add(rowIndex, _headerRowHeight);
                }
                else
                {
                    rowHeights[rowIndex] = Max(rowHeights[rowIndex], _headerRowHeight);
                }
            }
        }
    }
}
