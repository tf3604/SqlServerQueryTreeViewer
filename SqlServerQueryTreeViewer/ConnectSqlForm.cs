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
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bkh.ParseTreeLib;

namespace SqlServerQueryTreeViewer
{
    public partial class ConnectSqlForm : Form
    {
        private const string _winAuth = "Windows Authentication";
        private const string _sqlAuth = "SQL Server Authentication";

        private ApplicationSqlConnection _connection = null;

        public ConnectSqlForm()
        {
            InitializeComponent();
        }

        public ApplicationSqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        private ApplicationSqlConnection AcquireConnection()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = this.serverNameComboBox.Text;

            if (this.authenticationComboBox.Text == _winAuth)
            {
                sb.IntegratedSecurity = true;
            }
            else
            {
                sb.IntegratedSecurity = false;
                sb.UserID = this.userNameComboBox.Text;
                sb.Password = this.passwordTextBox.Text;
            }

            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(sb.ToString());
                sqlConnection.Open();
                return new ApplicationSqlConnection(sqlConnection);
            }
            catch
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Dispose();
                }
                throw;
            }
        }

        private void ConnectSqlForm_Load(object sender, EventArgs e)
        {
            this.authenticationComboBox.Items.Add(_winAuth);
            this.authenticationComboBox.Items.Add(_sqlAuth);
            this.authenticationComboBox.Text = _winAuth;

            this.serverNameComboBox.Items.AddRange(ViewerSettings.Instance.MostRecentSqlServers.ToArray());
            if (ViewerSettings.Instance.MostRecentSqlServers.Count > 0)
            {
                this.serverNameComboBox.Text = ViewerSettings.Instance.MostRecentSqlServers[0];
            }
        }

        private void AuthenticationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSqlAuth = (this.authenticationComboBox.Text == _sqlAuth);

            userNameLabel.Enabled = isSqlAuth;
            userNameComboBox.Enabled = isSqlAuth;
            passwordLabel.Enabled = isSqlAuth;
            passwordTextBox.Enabled = isSqlAuth;
            rememberPwdCheckBox.Enabled = isSqlAuth;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.serverNameComboBox.Text))
            {
                return;
            }

            Cursor oldCursor = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;
                _connection = this.AcquireConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "Error connecting to database: " + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = oldCursor;
            }

            ViewerSettings.Instance.AddMostRecentSqlServer(serverNameComboBox.Text);
            ViewerSettings.Instance.Save();
            this.DialogResult = DialogResult.OK;
        }
    }
}
