using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerParseTreeViewer
{
    public partial class ShowThumbnailForm : Form
    {
        private Image _image;

        public ShowThumbnailForm(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            InitializeComponent();
            _image = image;
        }

        private void ShowThumbnailForm_Load(object sender, EventArgs e)
        {
            thumbnailPictureBox.Image = _image;
            thumbnailPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
