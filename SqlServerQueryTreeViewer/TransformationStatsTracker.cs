//  Copyright(c) 2016-2019 Brian Hansen.

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

namespace SqlServerQueryTreeViewer
{
    internal class TransformationStatsTracker : InfoTracker
    {
        private static string _beforeTempTableName = "#xform_stats_6A1F9482466B48AD9717047A83EBFC56";
        private static string _afterTempTableName = "#xform_stats_9AB27FC62B814C76B6DEAF2FAE22396F";

        private static string _captureBeforeData = "select * into " + _beforeTempTableName + " from sys.dm_exec_query_transformation_stats;";
        private static string _captureAfterData = "select * into " + _afterTempTableName + " from sys.dm_exec_query_transformation_stats;";
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
                string sql = "select a.name, a.promised - b.promised promised, a.succeeded - b.succeeded succeeded from " +
                    _beforeTempTableName + " b join " + _afterTempTableName + " a on a.name = b.name where a.succeeded != b.succeeded order by name;";
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
