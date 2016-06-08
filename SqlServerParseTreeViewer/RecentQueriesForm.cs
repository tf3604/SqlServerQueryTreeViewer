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
                queryRichTextBox.Text = query.QueryText;
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
