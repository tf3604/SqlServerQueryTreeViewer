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

        private SqlConnection _connection;
        private StringBuilder _messages;
        private List<OutputMessage> _outputMessages;

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

            List<NodeIcon> nodeIcons;
            Bitmap bitmap = TreeVisualizer.Render(tree, out nodeIcons);
            subTab.DrawingSurface.Image = bitmap;
            subTab.TreeText = tree.InnerTreeText;
            subTab.SetIcons(nodeIcons);

            return tab;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            TabPage messagesTab = new TabPage(_tabTitleMessages);
            RichTextBox messagesTextBox = new RichTextBox();
            messagesTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            messagesTextBox.Size = messagesTab.Size;
            messagesTextBox.WordWrap = false;
            messagesTextBox.Multiline = true;
            messagesTextBox.ScrollBars = RichTextBoxScrollBars.Both;
            messagesTextBox.Font = new Font("Consolas", 10);
            messagesTab.Controls.Add(messagesTextBox);

            try
            {
                if (_connection == null ||
                    _connection.State != ConnectionState.Open)
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

                if (ViewerSettings.Instance.TrackOptimizerInfo)
                {
                    OptimizerInfoTracker.PreExecute(_connection);
                }

                if (ViewerSettings.Instance.TrackTransformationStats)
                {
                    TransformationStatsTracker.PreExecute(_connection);
                }

                List<string> sqlBatches = SplitSqlIntoBatches(sqlToExecute);
                List<DataTable> resultTables = new List<DataTable>();
                _messages = new StringBuilder();
                _outputMessages = new List<OutputMessage>();

                foreach (string sql in sqlBatches)
                {
                    using (DataSet resultSet = new DataSet())
                    {
                        using (SqlCommand command = new SqlCommand(sql, _connection))
                        {
                            _connection.InfoMessage += CaptureMessages;
                            _connection.FireInfoMessageEventOnUserErrors = true;

                            this.executeButton.Enabled = false;
                            Cursor oldCursor = this.Cursor;

                            try
                            {
                                this.Cursor = Cursors.WaitCursor;
                                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                {
                                    adapter.Fill(resultSet);
                                }

                                foreach (DataTable table in resultSet.Tables)
                                {
                                    resultTables.Add(table.Copy());
                                }
                            }
                            finally
                            {
                                _connection.InfoMessage -= CaptureMessages;
                                this.Cursor = oldCursor;
                                this.executeButton.Enabled = true;
                            }
                        }
                    }
                }

                if (ViewerSettings.Instance.TrackOptimizerInfo)
                {
                    OptimizerInfoTracker.PostExecute(_connection);
                }

                if (ViewerSettings.Instance.TrackTransformationStats)
                {
                    TransformationStatsTracker.PostExecute(_connection);
                }

                DataTable optimizerInfoTable = null;
                if (ViewerSettings.Instance.TrackOptimizerInfo)
                {
                    optimizerInfoTable = OptimizerInfoTracker.AnalyzeResults(_connection);
                }

                DataTable transformationStatsTable = null;
                if (ViewerSettings.Instance.TrackTransformationStats)
                {
                    transformationStatsTable = TransformationStatsTracker.AnalyzeResults(_connection);
                }

                string sqlOutput = _messages.ToString();

                TabPage pageToBeActivated = messagesTab;
                mainTabControl.TabPages.Add(messagesTab);

                foreach (OutputMessage message in _outputMessages)
                {
                    string text = message.Message + Environment.NewLine;
                    messagesTextBox.SelectionStart = messagesTextBox.TextLength;
                    messagesTextBox.SelectionLength = 0;

                    messagesTextBox.SelectionColor = message.IsErrorText ? Color.Red : Color.Black;
                    messagesTextBox.AppendText(text);
                }

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

                if (optimizerInfoTable != null)
                {
                    TabPage optimizerInfoTab = new TabPage("Optimizer");
                    mainTabControl.TabPages.Add(optimizerInfoTab);
                    Size gridSize = new Size(optimizerInfoTab.Width, optimizerInfoTab.Height);
                    DisplayTablesOnTabPage(optimizerInfoTab, new List<DataTable>() { optimizerInfoTable }, gridSize);
                }

                if (transformationStatsTable != null)
                {
                    TabPage transformationStatsTab = new TabPage("Transformations");
                    mainTabControl.TabPages.Add(transformationStatsTab);
                    Size gridSize = new Size(transformationStatsTab.Width, transformationStatsTab.Height);
                    DisplayTablesOnTabPage(transformationStatsTab, new List<DataTable>() { transformationStatsTable }, gridSize);
                }

                List<SqlParseTree> trees = new List<SqlParseTree>(TreeTextParser.Parse(sqlOutput));
                foreach (SqlParseTree tree in trees)
                {
                    TabPage tab = DrawTree(tree);
                    mainTabControl.TabPages.Add(tab);
                }

                mainTabControl.SelectedTab = pageToBeActivated;
            }
            catch (Exception ex)
            {
                messagesTextBox.SelectionStart = messagesTextBox.TextLength;
                messagesTextBox.SelectionLength = 0;

                messagesTextBox.SelectionColor = Color.Red;
                messagesTextBox.AppendText(ex.ToString());

                if (mainTabControl.Controls.Contains(messagesTab) == false)
                {
                    mainTabControl.Controls.Add(messagesTab);
                }
                mainTabControl.SelectedTab = messagesTab;
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

        private List<string> SplitSqlIntoBatches(string sqlToExecute)
        {
            List<string> batches = new List<string>();
            string pattern = @"^GO$";
            int startPos = 0;

            Match match = Regex.Match(sqlToExecute, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            while (match.Success)
            {
                string batch = sqlToExecute.Substring(startPos, match.Index - startPos);
                if (string.IsNullOrWhiteSpace(batch) == false)
                {
                    batches.Add(batch.Trim());
                }
                startPos = match.Index + match.Length;
                match = match.NextMatch();
            }

            if (startPos < sqlToExecute.Length)
            {
                string batch = sqlToExecute.Substring(startPos);
                if (string.IsNullOrWhiteSpace(batch) == false)
                {
                    batches.Add(batch.Trim());
                }
            }

            return batches;
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

        private void CaptureMessages(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError error in e.Errors)
            {
                OutputMessage message = new OutputMessage();
                message.IsErrorText = error.Class > 10;

                if (error.Class > 0)
                {
                    string errorInfo = string.Format("Msg {0}, Level {1}, State {2}, Line {3}", error.Number, error.Class, error.State, error.LineNumber);
                    _messages.AppendLine(errorInfo);
                    message.Message = errorInfo + Environment.NewLine + error.Message;
                }
                else
                {
                    _messages.AppendLine(error.Message);
                    message.Message = error.Message;
                }
                _outputMessages.Add(message);
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
                    _connection = connectForm.SqlConnection;
                    SetTraceFlags();
                    return true;
                }
            }
            return false;
        }

        private class OutputMessage
        {
            public string Message
            {
                get;
                set;
            }

            public bool IsErrorText
            {
                get;
                set;
            }
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
            else if (currentTab.Text == _tabTitleResults)
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

            queryRichTextBox.Text = ViewerSettings.Instance.CurrentQuery ?? string.Empty;
        }

        private void ViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ViewerSettings.Instance.AddRecentQuery(ViewerSettings.Instance.CurrentQuery);
            ViewerSettings.Instance.CurrentQuery = queryRichTextBox.Text;
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
                    ViewerSettings.Instance.AddRecentQuery(ViewerSettings.Instance.CurrentQuery);
                    ViewerSettings.Instance.Save();

                    string selectedQuery = form.SelectedQuery;
                    if (string.IsNullOrEmpty(selectedQuery) == false)
                    {
                        queryRichTextBox.Text = selectedQuery;
                    }
                }
            }
        }

        private void TabScript_Enter(object sender, EventArgs e)
        {
            ActiveControl = queryRichTextBox;
        }

        private void showThumbnailToolStripMenuItem_Click(object sender, EventArgs e)
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

            using (DataTable table = new DataTable())
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }

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

                // Handle trace flags that are enabled in options but not returned by dbcc tracestatus
                List<TraceFlag> traceFlags = ViewerSettings.Instance.TraceFlags.Where(tf => tf.Enabled && discoveredTraceFlags.Contains(tf.TraceFlagNumber) == false).ToList();
                traceFlags.ForEach(tf => ToggleTraceFlag(tf.TraceFlagNumber, true));
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

            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
