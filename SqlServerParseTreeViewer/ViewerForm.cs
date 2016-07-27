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
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using bkh.ParseTreeLib;

namespace SqlServerParseTreeViewer
{
    public partial class ViewerForm : Form
    {
        private const string _tabTitleScript = "Script";
        private const string _tabTitleMessages = "Messages";
        private const string _tabTitleResults = "Results";
        private const string _dialogFileFilter = "SQL Server Files (*.sql)|*.sql|All Files (*.*)|*.*";

        private ApplicationSqlConnection _connection;
        private QueryExecutionEngine _currentExecutionEngine = null;

        private TabPage _messagesTab;
        private RichTextBox _messagesTextBox;

        public ViewerForm()
        {
            InitializeComponent();
            tabScript.Enter += TabScript_Enter;
        }

        public TabPage DrawTree(SqlParseTree tree)
        {
            TabPage tab = new TabPage(tree.TreeDescription);

            ParseTreeTab subTab = new ParseTreeTab();
            subTab.Left = 0;
            subTab.Top = 0;
            subTab.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            tab.Controls.Add(subTab);
            tab.Size = mainTabControl.Size;
            subTab.Size = tab.Size;

            List<TreeNodeIcon> treeNodeIcons;
            Bitmap bitmap = TreeVisualizer.Render(tree, out treeNodeIcons);
            subTab.DrawingSurface.Image = bitmap;
            subTab.TreeText = tree.InnerTreeText;
            subTab.SetIcons(treeNodeIcons.ConvertAll(i => i as NodeIcon));

            return tab;
        }

        public TabPage DrawMemo(SqlMemo memo)
        {
            TabPage tab = new TabPage(memo.Description);

            ParseTreeTab subTab = new ParseTreeTab();
            subTab.Left = 0;
            subTab.Top = 0;
            subTab.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            subTab.IsMemo = true;

            tab.Controls.Add(subTab);
            tab.Size = mainTabControl.Size;
            subTab.Size = tab.Size;

            List<MemoNodeIcon> memoNodeIcons;
            Bitmap bitmap = MemoVisualizer.Render(memo, out memoNodeIcons);
            subTab.DrawingSurface.Image = bitmap;
            subTab.TreeText = memo.InnerText;
            subTab.SetIcons(memoNodeIcons.ConvertAll(i => i as NodeIcon));

            return tab;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            _messagesTab = new TabPage(_tabTitleMessages);
            _messagesTextBox = new RichTextBox();
            _messagesTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            _messagesTextBox.Size = _messagesTab.Size;
            _messagesTextBox.WordWrap = false;
            _messagesTextBox.Multiline = true;
            _messagesTextBox.ScrollBars = RichTextBoxScrollBars.Both;
            _messagesTextBox.Font = new Font("Consolas", 10);
            _messagesTab.Controls.Add(_messagesTextBox);

            try
            {
                if (_connection == null ||
                    _connection.IsAvailable == false)
                {
                    if (ConnectDatabase() == false)
                    {
                        return;
                    }
                }

                string sqlToExecute = queryRichTextBox.SelectedText;
                if (string.IsNullOrEmpty(sqlToExecute))
                {
                    sqlToExecute = queryRichTextBox.Text;
                }

                ViewerSettings.Instance.CurrentQuery = queryRichTextBox.Text;
                ViewerSettings.Instance.CurrentQueryRtf = queryRichTextBox.Rtf;
                ViewerSettings.Instance.Save();

                // Clear all but the script tab
                List<TabPage> tabsToDelete = new List<TabPage>();
                foreach (TabPage tab in mainTabControl.TabPages)
                {
                    if (tab.Text != _tabTitleScript)
                    {
                        tabsToDelete.Add(tab);
                    }
                }
                tabsToDelete.ForEach(t => mainTabControl.TabPages.Remove(t));

                if (ViewerSettings.Instance.TraceFlagListHasBeenEdited)
                {
                    SetTraceFlags();
                }

                this.executeButton.Enabled = false;
                this.connectButton.Enabled = false;
                this.cancelQueryButton.Enabled = true;
                this.executionStatus.Text = "Executing query";
                Cursor oldCursor = this.Cursor;

                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    _currentExecutionEngine = new QueryExecutionEngine(_connection, sqlToExecute);
                    _currentExecutionEngine.ExecuteComplete += OnExecuteComplete;
                    _currentExecutionEngine.StartExecution();
                }
                finally
                {
                    this.Cursor = oldCursor;
                }

            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }

        private void OnExecuteComplete(object sender, SqlExecuteCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, SqlExecuteCompleteEventArgs>(DisplayResults), sender, e);
            }
            else
            {
                DisplayResults(sender, e);
            }

            _currentExecutionEngine = null;
        }

        private void DisplayResults(object sender, SqlExecuteCompleteEventArgs e)
        {
            if (e.Exception == null)
            {
                DisplayNormalResults(sender, e);
            }
            else
            {
                DisplayExceptionResults(sender, e);
            }

            this.executeButton.Enabled = true;
            this.connectButton.Enabled = true;
            this.cancelQueryButton.Enabled = false;
        }

        private void DisplayExceptionResults(object sender, SqlExecuteCompleteEventArgs e)
        {
            DisplayException(e.Exception);
        }

        private void DisplayException(Exception ex)
        {
            _messagesTextBox.SelectionStart = _messagesTextBox.TextLength;
            _messagesTextBox.SelectionLength = 0;

            _messagesTextBox.SelectionColor = Color.Red;
            _messagesTextBox.AppendText(ex.ToString());

            if (mainTabControl.Controls.Contains(_messagesTab) == false)
            {
                mainTabControl.Controls.Add(_messagesTab);
            }
            mainTabControl.SelectedTab = _messagesTab;
            executionStatus.Text = "Unexpected error.";
        }

        private void DisplayNormalResults(object sender, SqlExecuteCompleteEventArgs e)
        {
            string sqlOutput = _currentExecutionEngine.Messages;

            TabPage pageToBeActivated = _messagesTab;
            mainTabControl.TabPages.Add(_messagesTab);
            bool hasError = false;

            foreach (OutputMessage message in _currentExecutionEngine.OutputMessages)
            {
                string text = message.Message + Environment.NewLine;
                _messagesTextBox.SelectionStart = _messagesTextBox.TextLength;
                _messagesTextBox.SelectionLength = 0;

                _messagesTextBox.SelectionColor = message.IsErrorText ? Color.Red : Color.Black;
                _messagesTextBox.AppendText(text);
                hasError ^= message.IsErrorText;
            }

            List<DataTable> resultTables = _currentExecutionEngine.ResultTables.ToList();
            if (resultTables.Count > 0)
            {
                TabPage resultsTab = new TabPage(_tabTitleResults);
                pageToBeActivated = resultsTab;
                mainTabControl.TabPages.Add(resultsTab);

                StatusBar statusBar = new StatusBar();
                int statusBarHeight = 20;
                Size gridSize = new Size(resultsTab.Width, (resultsTab.Height - statusBarHeight) / resultTables.Count);
                statusBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
                statusBar.Size = new Size(resultsTab.Width, statusBarHeight);
                statusBar.Top = gridSize.Height * resultTables.Count;
                resultsTab.Controls.Add(statusBar);

                long rowCount = DisplayTablesOnTabPage(resultsTab, resultTables, gridSize);
                statusBar.Text = string.Format("{0} resultset(s); {1:#,##0} row(s) total", resultTables.Count, rowCount);
                resultsTab.Resize += ResultsTab_Resize;
            }

            if (_currentExecutionEngine.OptimizerInfoTable != null)
            {
                TabPage optimizerInfoTab = new TabPage("Optimizer");
                mainTabControl.TabPages.Add(optimizerInfoTab);
                Size gridSize = new Size(optimizerInfoTab.Width, optimizerInfoTab.Height);
                DisplayTablesOnTabPage(optimizerInfoTab, new List<DataTable>() { _currentExecutionEngine.OptimizerInfoTable }, gridSize);
            }

            if (_currentExecutionEngine.TransformationStatsTable != null)
            {
                TabPage transformationStatsTab = new TabPage("Transformations");
                mainTabControl.TabPages.Add(transformationStatsTab);
                Size gridSize = new Size(transformationStatsTab.Width, transformationStatsTab.Height);
                DisplayTablesOnTabPage(transformationStatsTab, new List<DataTable>() { _currentExecutionEngine.TransformationStatsTable }, gridSize);
            }

            List<SqlParseTree> trees = new List<SqlParseTree>(TreeTextParser.Parse(sqlOutput));
            List<SqlMemo> memos = new List<SqlMemo>(MemoTextParser.Parse(sqlOutput));

            foreach (SqlParseTree tree in trees)
            {
                TabPage tab = DrawTree(tree);
                mainTabControl.TabPages.Add(tab);
            }

            foreach (SqlMemo memo in memos)
            {
                TabPage tab = DrawMemo(memo);
                mainTabControl.TabPages.Add(tab);
            }

            mainTabControl.SelectedTab = pageToBeActivated;

            if (hasError)
            {
                executionStatus.Text = "Query completed with errors.";
            }
            else if (e.CancelledByUser)
            {
                executionStatus.Text = "Query was cancelled by user.";
            }
            else
            {
                executionStatus.Text = "Query executed successfully.";
            }
        }

        private long DisplayTablesOnTabPage(TabPage tabPage, List<DataTable> tables, Size gridSize)
        {
            long rowCount = 0;
            for (int tableIndex = 0; tableIndex < tables.Count; tableIndex++)
            {
                DataTable resultTable = tables[tableIndex];
                FixupDataTable(resultTable);

                DataGridView grid = new DataGridView();
                grid.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                grid.Size = gridSize;
                grid.Left = 0;
                grid.Top = gridSize.Height * tableIndex;
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.RowHeadersVisible = false;
                grid.ReadOnly = true;
                grid.DataSource = resultTable;
                grid.AutoResizeColumns();
                tabPage.Controls.Add(grid);

                rowCount += resultTable.Rows.Count;
            }

            return rowCount;
        }

        private void FixupDataTable(DataTable table)
        {
            // Any columns that are of type byte[] need to be replaced with a string equivalent; otherwise
            // the grid will throw an error when binding the table as the data source.

            List<DataColumn> byteColumns = table.Columns.Cast<DataColumn>().Where(c => c.DataType == typeof(byte[])).ToList();
            foreach (DataColumn column in byteColumns)
            {
                string newColumnName = "tempCol" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                DataColumn newColumn = table.Columns.Add(newColumnName, typeof(string));
                newColumn.SetOrdinal(column.Ordinal);

                foreach (DataRow row in table.Rows)
                {
                    row[newColumn] = GetStringValue(row[column], string.Empty, string.Empty);
                }

                table.Columns.Remove(column);
                newColumn.ColumnName = column.ColumnName;
            }
        }

        private void ResultsTab_Resize(object sender, EventArgs e)
        {
            TabPage resultsTab = sender as TabPage;
            StatusBar statusBar = null;
            if (resultsTab != null)
            {
                List<DataGridView> grids = new List<DataGridView>();
                foreach (Control control in resultsTab.Controls)
                {
                    DataGridView grid = control as DataGridView;
                    if (grid != null)
                    {
                        grids.Add(grid);
                    }

                    if (control is StatusBar)
                    {
                        statusBar = control as StatusBar;
                    }
                }

                grids.Sort((x, y) => x.Top.CompareTo(y.Top));
                Size sizePerGrid = new Size(resultsTab.Width, (resultsTab.Height - statusBar?.Height ?? 0) / grids.Count);

                for (int gridIndex = 0; gridIndex < grids.Count; gridIndex++)
                {
                    DataGridView grid = grids[gridIndex];
                    grid.Top = resultsTab.Height / grids.Count * gridIndex;
                    grid.Size = sizePerGrid;
                }
            }
        }

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }

        private bool ConnectDatabase()
        {
            using (ConnectSqlForm connectForm = new ConnectSqlForm())
            {
                DialogResult result = connectForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _connection = connectForm.Connection;
                    SetTraceFlags();
                    return true;
                }
            }
            return false;
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsForm form = new OptionsForm())
            {
                form.ShowDialog();
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = mainTabControl.SelectedTab;
            if (currentTab.Text == _tabTitleScript)
            {
                Clipboard.SetText(queryRichTextBox.Text);
            }
            else if (currentTab.Text == _tabTitleMessages)
            {
                RichTextBox messagesTextBox = FindFirstControlOfType<RichTextBox>(currentTab.Controls);
                if (messagesTextBox != null)
                {
                    Clipboard.SetText(messagesTextBox.Text);
                }
            }
            else if (currentTab.Text == _tabTitleResults ||
                FindFirstControlOfType<DataGridView>(currentTab.Controls) != null)
            {
                List<DataGridView> grids = FindControlsOfType<DataGridView>(currentTab.Controls);
                if (grids != null)
                {
                    StringBuilder sb = new StringBuilder();
                    string delimiter = "\t";
                    string qualifier = "\"";
                    string rowSeparator = Environment.NewLine;

                    foreach (DataGridView grid in grids)
                    {
                        if (grid.DataSource is DataTable)
                        {
                            sb.Append(TableToDelimitedString(grid.DataSource as DataTable, delimiter, qualifier, rowSeparator));
                        }
                        else if (grid.DataSource is DataSet)
                        {
                            foreach (DataTable table in (grid.DataSource as DataSet).Tables)
                            {
                                sb.Append(TableToDelimitedString(table, delimiter, qualifier, rowSeparator));
                            }
                        }
                    }

                    Clipboard.SetText(sb.ToString());
                }
            }
            else
            {
                ParseTreeTab treeTab = FindFirstControlOfType<ParseTreeTab>(currentTab.Controls);
                if (treeTab != null)
                {
                    Clipboard.SetImage(treeTab.DrawingSurface.Image);
                }
            }
        }

        private T FindFirstControlOfType<T>(System.Windows.Forms.Control.ControlCollection controls) where T : class
        {
            List<T> matches = FindControlsOfType<T>(controls);
            if (matches.Count > 0)
            {
                return matches[0];
            }

            return null;
        }

        private List<T> FindControlsOfType<T>(System.Windows.Forms.Control.ControlCollection controls) where T : class
        {
            List<T> matches = new List<T>();
            foreach (Control control in controls)
            {
                if (control is T)
                {
                    matches.Add(control as T);
                }
            }

            return matches;
        }

        private string TableToDelimitedString(DataTable table, string delimiter, string qualifier, string rowSeparator)
        {
            StringBuilder sb = new StringBuilder();
            bool isFirstColumn = true;

            foreach (DataColumn column in table.Columns)
            {
                if (isFirstColumn)
                {
                    isFirstColumn = false;
                }
                else
                {
                    sb.Append(delimiter);
                }
                sb.Append(GetStringValue(column.ColumnName, delimiter, qualifier));
            }
            sb.Append(rowSeparator);

            foreach (DataRow row in table.Rows)
            {
                isFirstColumn = true;
                foreach (DataColumn column in table.Columns)
                {
                    if (isFirstColumn)
                    {
                        isFirstColumn = false;
                    }
                    else
                    {
                        sb.Append(delimiter);
                    }
                    sb.Append(GetStringValue(row[column], delimiter, qualifier));
                }
                sb.Append(rowSeparator);
            }

            return sb.ToString();
        }

        private string GetStringValue(object obj, string delimiter, string qualifier)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            Type type = obj.GetType();
            string value = string.Empty;
            if (type == typeof(string))
            {
                value = obj as string;
            }
            else if (type == typeof(int) ||
                type == typeof(long) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(ushort) ||
                type == typeof(sbyte) ||
                type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(decimal) ||
                type == typeof(Guid))
            {
                value = obj.ToString();
            }
            else if (type == typeof(bool))
            {
                value = ((bool)obj) ? "1" : "0";
            }
            else if (type == typeof(byte[]))
            {
                byte[] array = obj as byte[];
                value = "0x" + BitConverter.ToString(array).Replace("-", string.Empty);
            }
            else
            {
                value = "UNKNOWN";
            }

            if (string.IsNullOrEmpty(delimiter) == false &&
                value.ToLowerInvariant().Contains(delimiter.ToLowerInvariant()))
            {
                value = qualifier + value + qualifier;
            }

            return value;
        }

        private void ViewerForm_Load(object sender, EventArgs e)
        {
            if (ViewerSettings.Instance.UserAgreesToDisclaimer == false)
            {
                using (DisclaimerForm disclaimer = new DisclaimerForm())
                {
                    if (disclaimer.ShowDialog() == DialogResult.Cancel)
                    {
                        this.Close();
                    }
                    else
                    {
                        ViewerSettings.Instance.UserAgreesToDisclaimer = true;
                        ViewerSettings.Instance.Save();
                    }
                }
            }

            if (ViewerSettings.Instance.CurrentQueryRtf != null)
            {
                queryRichTextBox.Rtf = ViewerSettings.Instance.CurrentQueryRtf;
            }
            else
            {
                queryRichTextBox.Text = ViewerSettings.Instance.CurrentQuery ?? string.Empty;
            }
        }

        private void ViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ViewerSettings.Instance.AddRecentQuery(ViewerSettings.Instance.CurrentQuery, ViewerSettings.Instance.CurrentQueryRtf);
            ViewerSettings.Instance.CurrentQuery = queryRichTextBox.Text;
            ViewerSettings.Instance.CurrentQueryRtf = queryRichTextBox.Rtf;
            ViewerSettings.Instance.Save();
        }

        private string CleanupRtf(string rtf)
        {
            string pattern = "\\{\\\\pict|\\{\\\\object";
            Match match = Regex.Match(rtf, pattern);
            while (match.Success)
            {
                int count = 1;
                for (int i = match.Index + 2; i <= rtf.Length; i++)
                {
                    if (rtf[i] == '{')
                    {
                        count += 1;
                    }
                    else if (rtf[i] == '}')
                    {
                        count -= 1;
                    }

                    if (count == 0)
                    {
                        rtf = rtf.Remove(match.Index, i - match.Index + 1);
                        break;
                    }
                }
                match = Regex.Match(rtf, pattern);
            }
            return rtf;
        }

        private void QueryRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool isF5 = e.KeyCode == Keys.F5 &&
                e.Alt == false &&
                e.Control == false &&
                e.Shift == false;

            bool isCtrlE = e.KeyCode == Keys.E &&
                e.Alt == false &&
                e.Control == true &&
                e.Shift == false;

            bool isAltX = e.KeyCode == Keys.X &&
                e.Alt == true &&
                e.Control == false &&
                e.Shift == false;

            if (isF5 || isCtrlE || isAltX)
            {
                e.Handled = true;
                Execute();
            }

            bool isCtrlV = e.KeyCode == Keys.V &&
                e.Alt == false &&
                e.Control == true &&
                e.Shift == false;

            if (isCtrlV)
            {
                if (Clipboard.ContainsData(DataFormats.Rtf))
                {
                    // Strip out images and objects embedded in RTF
                    string rtf;
                    using (RichTextBox tempRichTextBox = new RichTextBox())
                    {
                        tempRichTextBox.Rtf = Clipboard.GetData(DataFormats.Rtf) as string;
                        rtf = tempRichTextBox.Rtf;
                    }
                    queryRichTextBox.SelectedRtf = CleanupRtf(rtf);
                    e.Handled = true;
                }
                else if (Clipboard.ContainsText())
                {
                    queryRichTextBox.SelectedText = Clipboard.GetText();
                    e.Handled = true;
                }
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = _dialogFileFilter;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string queryText = File.ReadAllText(dialog.FileName);
                        queryRichTextBox.Text = queryText;

                        ViewerSettings.Instance.CurrentFileName = dialog.FileName;
                        ViewerSettings.Instance.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to open file", MessageBoxButtons.OK);
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            queryRichTextBox.Text = string.Empty;
            ViewerSettings.Instance.CurrentFileName = string.Empty;
            ViewerSettings.Instance.Save();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ViewerSettings.Instance.CurrentFileName))
                {
                    SaveAsToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    File.WriteAllText(ViewerSettings.Instance.CurrentFileName, queryRichTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to save file", MessageBoxButtons.OK);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    if (string.IsNullOrEmpty(ViewerSettings.Instance.CurrentFileName) == false)
                    {
                        dialog.InitialDirectory = Path.GetDirectoryName(ViewerSettings.Instance.CurrentFileName);
                        dialog.FileName = Path.GetFileName(ViewerSettings.Instance.CurrentFileName);
                    }

                    dialog.Filter = _dialogFileFilter;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dialog.FileName, queryRichTextBox.Text);
                        ViewerSettings.Instance.CurrentFileName = dialog.FileName;
                        ViewerSettings.Instance.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to save file", MessageBoxButtons.OK);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm form = new AboutForm())
            {
                form.ShowDialog();
            }
        }

        private void RecentQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RecentQueriesForm form = new RecentQueriesForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ViewerSettings.Instance.AddRecentQuery(ViewerSettings.Instance.CurrentQuery, ViewerSettings.Instance.CurrentQueryRtf);
                    ViewerSettings.Instance.Save();

                    string selectedQuery = form.SelectedQuery;
                    string selectedQueryRtf = form.SelectedQueryRtf;

                    if (string.IsNullOrEmpty(selectedQueryRtf) == false)
                    {
                        queryRichTextBox.Rtf = selectedQueryRtf;
                    }
                    else if (string.IsNullOrEmpty(selectedQuery) == false)
                    {
                        queryRichTextBox.Clear();
                        queryRichTextBox.Text = selectedQuery;
                    }
                }
            }
        }

        private void TabScript_Enter(object sender, EventArgs e)
        {
            ActiveControl = queryRichTextBox;
        }

        private void ShowThumbnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage currentTab = mainTabControl.SelectedTab;
            ParseTreeTab treeTab = FindFirstControlOfType<ParseTreeTab>(currentTab.Controls);
            if (treeTab != null)
            {
                Image image = treeTab.DrawingSurface.Image;
                ShowThumbnailForm thumbnailForm = new ShowThumbnailForm(image);
                thumbnailForm.ShowDialog();
            }
        }

        private void SetTraceFlags()
        {
            string sql = @"dbcc tracestatus();";

            using (Dal dal = new Dal(_connection))
            {
                using (DataTable table = dal.ExecuteQueryOneResultSet(sql))
                {
                    List<int> discoveredTraceFlags = new List<int>();
                    foreach (DataRow row in table.Rows)
                    {
                        int traceFlagNumber = (int)(short)row["TraceFlag"];
                        bool enabledForSession = (short)row["Session"] != 0;

                        discoveredTraceFlags.Add(traceFlagNumber);
                        TraceFlag traceFlag = ViewerSettings.Instance.TraceFlags.FirstOrDefault(tf => tf.TraceFlagNumber == traceFlagNumber);
                        if (traceFlag != null)
                        {
                            if (traceFlag.Enabled != enabledForSession)
                            {
                                ToggleTraceFlag(traceFlagNumber, traceFlag.Enabled);
                            }
                        }
                    }

                    if (ViewerSettings.Instance.TraceFlags != null)
                    {
                        // Handle trace flags that are enabled in options but not returned by dbcc tracestatus
                        List<TraceFlag> traceFlags = ViewerSettings.Instance.TraceFlags.Where(tf => tf.Enabled && discoveredTraceFlags.Contains(tf.TraceFlagNumber) == false).ToList();
                        traceFlags.ForEach(tf => ToggleTraceFlag(tf.TraceFlagNumber, true));
                    }
                }
            }
        }

        private void ToggleTraceFlag(int traceFlagNumber, bool enable)
        {
            string sql;
            if (enable)
            {
                sql = string.Format("dbcc traceon({0});", traceFlagNumber);
            }
            else
            {
                sql = string.Format("dbcc traceoff({0});", traceFlagNumber);
            }

            using (Dal dal = new Dal(_connection))
            {
                dal.ExecuteQueryNoResultSets(sql);
            }
        }

        private void CancelQueryButton_Click(object sender, EventArgs e)
        {
            QueryExecutionEngine executionEngine = _currentExecutionEngine;
            if (executionEngine != null)
            {
                executionEngine.CancelQuery();
            }
        }
    }
}
