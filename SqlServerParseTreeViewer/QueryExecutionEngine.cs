using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerParseTreeViewer
{
    internal class QueryExecutionEngine
    {
        private string _sql;
        private ApplicationSqlConnection _connection;
        private StringBuilder _messages;
        private List<OutputMessage> _outputMessages;
        private List<DataTable> _resultTables;
        private DataTable _optimizerInfoTable;
        private DataTable _transformationStatsTable;
        private bool _isCancelled;
        private Dal _dal;

        public EventHandler<SqlExecuteCompleteEventArgs> ExecuteComplete;

        public QueryExecutionEngine(ApplicationSqlConnection connection, string sql)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            _sql = sql ?? string.Empty;
            _connection = connection;
        }

        public string Messages
        {
            get
            {
                return _messages.ToString();
            }
        }

        public ReadOnlyCollection<OutputMessage> OutputMessages
        {
            get
            {
                return new ReadOnlyCollection<OutputMessage>(_outputMessages);
            }
        }

        public ReadOnlyCollection<DataTable> ResultTables
        {
            get
            {
                return new ReadOnlyCollection<DataTable>(_resultTables);
            }
        }

        public DataTable OptimizerInfoTable
        {
            get
            {
                return _optimizerInfoTable;
            }
        }

        public DataTable TransformationStatsTable
        {
            get
            {
                return _transformationStatsTable;
            }
        }

        public void StartExecution()
        {
            Thread workerThread = new Thread(new ThreadStart(ExecuteAllSql));

            workerThread.IsBackground = true;
            workerThread.Start();
        }

        private void ExecuteAllSql()
        {
            Exception exception = null;
            _isCancelled = false;

            try
            {
                List<string> sqlBatches = SplitSqlIntoBatches(_sql);
                _resultTables = new List<DataTable>();
                _messages = new StringBuilder();
                _outputMessages = new List<OutputMessage>();

                InitializeTrackers();

                using (Dal dal = new Dal(_connection))
                {
                    _dal = dal;
                    try
                    {
                        foreach (string sql in sqlBatches)
                        {
                            _connection.InfoMessage += CaptureMessages;
                            _connection.FireInfoMessageEventOnUserErrors = true;

                            try
                            {
                                using (DataSet resultSet = dal.ExecuteQueryMultipleResultSets(sql))
                                {
                                    foreach (DataTable table in resultSet.Tables)
                                    {
                                        _resultTables.Add(table.Copy());
                                    }
                                }
                            }
                            finally
                            {
                                _connection.InfoMessage -= CaptureMessages;
                            }

                            if (_isCancelled)
                            {
                                break;
                            }
                        }
                    }
                    finally
                    {
                        _dal = null;
                    }
                }

                FinalizeTrackers();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            EventHandler<SqlExecuteCompleteEventArgs> executeCompleteHandler = this.ExecuteComplete;
            if (executeCompleteHandler != null)
            {
                SqlExecuteCompleteEventArgs args = new SqlExecuteCompleteEventArgs();
                args.Exception = exception;
                args.CancelledByUser = _isCancelled;
                executeCompleteHandler(this, args);
            }
        }

        public void CancelQuery()
        {
            _isCancelled = true;
            Dal dal = _dal;
            if (dal != null)
            {
                dal.Cancel();
            }
        }

        private static List<string> SplitSqlIntoBatches(string sqlToExecute)
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

        private void InitializeTrackers()
        {
            if (ViewerSettings.Instance.TrackOptimizerInfo)
            {
                OptimizerInfoTracker.PreExecute(_connection);
            }

            if (ViewerSettings.Instance.TrackTransformationStats)
            {
                TransformationStatsTracker.PreExecute(_connection);
            }
        }

        private void FinalizeTrackers()
        {
            if (ViewerSettings.Instance.TrackOptimizerInfo)
            {
                OptimizerInfoTracker.PostExecute(_connection);
            }

            if (ViewerSettings.Instance.TrackTransformationStats)
            {
                TransformationStatsTracker.PostExecute(_connection);
            }

            _optimizerInfoTable = null;
            if (ViewerSettings.Instance.TrackOptimizerInfo)
            {
                _optimizerInfoTable = OptimizerInfoTracker.AnalyzeResults(_connection);
            }

            _transformationStatsTable = null;
            if (ViewerSettings.Instance.TrackTransformationStats)
            {
                _transformationStatsTable = TransformationStatsTracker.AnalyzeResults(_connection);
            }
        }
    }
}
