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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace SqlServerParseTreeViewer
{
    internal class OptimizerInfoTracker : InfoTracker
    {
        private static string _beforeTempTableName = "#opt_info_7A70EBC47D934D4E9C0C8645CCE56D10";
        private static string _afterTempTableName = "#opt_info_B34F0C204E08425199739B4F77AEE039";

        private static string _captureBeforeData = "select * into " + _beforeTempTableName + " from sys.dm_exec_query_optimizer_info;";
        private static string _captureAfterData = "select * into " + _afterTempTableName + " from sys.dm_exec_query_optimizer_info;";
        private static string _dropBeforeTable = string.Format("if object_id('tempdb..{0}') is not null drop table {0};", _beforeTempTableName);
        private static string _dropAfterTable = string.Format("if object_id('tempdb..{0}') is not null drop table {0};", _afterTempTableName);
        private static string _dropTables = _dropBeforeTable + _dropAfterTable;

        public static void PreExecute(ApplicationSqlConnection connection)
        {
            using (Dal dal = new Dal(connection))
            {
                // Run the before and after scripts once just to make sure they are in the plan cache and don't skew the results
                dal.ExecuteQueryNoResultSets(_captureBeforeData);
                dal.ExecuteQueryNoResultSets(_captureAfterData);

                // Drop the temporary tables
                dal.ExecuteQueryNoResultSets(_dropTables);

                // Capture the before data
                dal.ExecuteQueryNoResultSets(_captureBeforeData);
            }
        }

        public static void PostExecute(ApplicationSqlConnection connection)
        {
            using (Dal dal = new Dal(connection))
            {
                // Capture the after data
                dal.ExecuteQueryNoResultSets(_captureAfterData);
            }
        }

        public static DataTable AnalyzeResults(ApplicationSqlConnection connection)
        {
            using (Dal dal = new Dal(connection))
            {
                // Query the differences
                string sql = "select a.counter, a.occurrence - b.occurrence occurrence, a.occurrence * a.value - b.occurrence * b.value value from " +
                    _beforeTempTableName + " b join " + _afterTempTableName + " a on a.counter = b.counter where a.occurrence != b.occurrence order by counter;";
                DataTable differenceTable = dal.ExecuteQueryOneResultSet(sql);

                try
                {
                    // Drop the temporary tables
                    dal.ExecuteQueryNoResultSets(_dropTables);

                    return differenceTable;
                }
                catch
                {
                    if (differenceTable != null)
                    {
                        differenceTable.Dispose();
                        differenceTable = null;
                    }
                    throw;
                }
            }
        }
    }
}
