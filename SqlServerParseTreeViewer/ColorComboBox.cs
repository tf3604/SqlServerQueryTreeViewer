using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    internal class ColorComboBox : ComboBox
    {
        public ColorComboBox()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                if (Items[e.Index] is Color)
                {
                    Color color = (Color)Items[e.Index];
                    DropDownItem item = new DropDownItem(color);
                    e.Graphics.DrawImage(item.Image, e.Bounds.Left, e.Bounds.Top);
                    e.Graphics.DrawString(
                        item.ToString(),
                        e.Font,
                        new SolidBrush(e.ForeColor),
                        e.Bounds.Left + item.Image.Width,
                        e.Bounds.Top + 2);
                }
            }

            base.OnDrawItem(e);
        }

        public class DropDownItem
        {
            public Color Value
            {
                get;
                set;
            }

            public Image Image
            {
                get;
                set;
            }

            public DropDownItem(Color color)
            {
                Value = color;
                Image = new Bitmap(16, 16);
                using (Graphics graphics = Graphics.FromImage(Image))
                {
                    using (Brush brush = new SolidBrush(color))
                    {
                        graphics.DrawRectangle(Pens.White, 0, 0, Image.Width, Image.Height);
                        graphics.FillRectangle(brush, 1, 1, Image.Width - 1, Image.Height - 1);
                    }
                }
            }

            public override string ToString()
            {
                if (Value.IsKnownColor)
                {
                    return Value.Name;
                }

                return string.Format("R={0}, G={1}, B={2}", Value.R, Value.G, Value.B);
            }
        }
    }
}
