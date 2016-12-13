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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerQueryTreeViewer
{
    public partial class OptionsForm : Form
    {
        private UserControl _currentUserControl;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            List<OptionsPage> pages = new List<OptionsPage>()
            {
                new OptionsPage("Counters", new InfoTrackerUserControl()),
                new OptionsPage("Trace Flags", new TraceFlagUserControl()),
                new OptionsPage("Operator Colors", new OperatorColorsUserControl()),
                new OptionsPage("Render Style", new RenderStyleUserControl())
            };

            this.pagesListBox.Items.Clear();
            this.pagesListBox.Items.AddRange(pages.ToArray());

            this.pagesListBox.SelectedIndex = 0;
        }

        public class OptionsPage
        {
            public OptionsPage(string displayText, UserControl pageControl)
            {
                DisplayText = displayText;
                PageControl = pageControl;
            }

            public string DisplayText
            {
                get;
                private set;
            }

            public UserControl PageControl
            {
                get;
                private set;
            }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ViewerSettings.CancelClone();
            DialogResult = DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ViewerSettings.PromoteClone();
            ViewerSettings.Instance.Save();
            DialogResult = DialogResult.OK;
        }

        private void PagesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsPage optionsPage = pagesListBox.SelectedItem as OptionsPage;
            UserControl page = optionsPage.PageControl;

            page.Left = pagesListBox.Left + pagesListBox.Width + 15;
            page.Top = pagesListBox.Top;
            page.Width = this.Width - page.Left - 15;
            page.Height = pagesListBox.Height;

            if (_currentUserControl != null)
            {
                this.Controls.Remove(_currentUserControl);
            }

            _currentUserControl = page;
            this.Controls.Add(_currentUserControl);
        }
    }
}
