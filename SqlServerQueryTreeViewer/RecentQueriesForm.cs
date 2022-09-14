//  Copyright(c) 2016-2022 Breanna Hansen.

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
    public partial class RecentQueriesForm : Form
    {
        public RecentQueriesForm()
        {
            InitializeComponent();
        }

        public string SelectedQuery
        {
            get
            {
                SubmittedQueryInfo query = recentQueryListBox.SelectedItem as SubmittedQueryInfo;
                if (query != null)
                {
                    return query.QueryText;
                }

                return null;
            }
        }

        public string SelectedQueryRtf
        {
            get
            {
                SubmittedQueryInfo query = recentQueryListBox.SelectedItem as SubmittedQueryInfo;
                if (query != null)
                {
                    return query.QueryTextRtf;
                }

                return null;
            }
        }

        private void RecentQueriesForm_Load(object sender, EventArgs e)
        {
            recentQueryListBox.Items.Clear();
            ViewerSettings.Instance.RecentQueries.ToList().ForEach(q => recentQueryListBox.Items.Add(q));
            if (recentQueryListBox.Items.Count > 0)
            {
                recentQueryListBox.SelectedIndex = 0;
            }
        }

        private void RecentQueryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SubmittedQueryInfo query = recentQueryListBox.SelectedItem as SubmittedQueryInfo;
            if (query != null)
            {
                if (string.IsNullOrEmpty(query.QueryTextRtf) == false)
                {
                    queryRichTextBox.Rtf = query.QueryTextRtf;
                }
                else
                {
                    queryRichTextBox.Clear();
                    queryRichTextBox.Text = query.QueryText;
                }
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
