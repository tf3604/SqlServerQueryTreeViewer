using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    internal static class TreeLabelGenerator
    {
        public static string GetLabel(SqlParseTreeNode node)
        {
            if (node.OperationName == "LogOp_Get")
            {
                string tableName = ExtractTableNameAndAlias(node);
                return $"Get {tableName}";
            }

            if (node.OperationName == "ScaOp_Identifier")
            {
                string columnName = ExtractColumnName(node);
                return $"{columnName}";
            }

            if (node.OperationName == "ScaOp_Const")
            {
                string constant = ExtractConstant(node);
                return $"{constant}";
            }

            return node.OperationName;
        }

        private static string ExtractTableNameAndAlias(SqlParseTreeNode node)
        {
            // Quick and dirty; probably has some cases where this isn't going to work
            if (string.IsNullOrEmpty(node.Arguments))
            {
                return string.Empty;
            }

            string tableLabel = "TBL: ";
            string aliasLabel = "alias TBL: ";

            int tableLabelIndex = node.Arguments.IndexOf(tableLabel, StringComparison.InvariantCultureIgnoreCase);
            if (tableLabelIndex < 0)
            {
                return string.Empty;
            }

            bool inEscape = false;
            bool hasAlias = false;
            int aliasStartIndex = 0;
            StringBuilder sb = new StringBuilder();

            for (int index = tableLabelIndex + tableLabel.Length; index < node.Arguments.Length; index++)
            {
                char c = node.Arguments[index];
                if (c == '[')
                {
                    inEscape = true;
                }
                if (c == ']' && inEscape == true)
                {
                    inEscape = false;
                }
                if (c == ' ' && inEscape == false)
                {
                    break;
                }
                if (c == '(' && inEscape == false)
                {
                    if (node.Arguments.Substring(index + 1).StartsWith(aliasLabel, StringComparison.InvariantCultureIgnoreCase))
                    {
                        hasAlias = true;
                        aliasStartIndex = index + aliasLabel.Length + 1;
                    }
                    break;
                }
                sb.Append(c);
            }

            if (hasAlias)
            {
                int closeParenIndex = node.Arguments.IndexOf(")", aliasStartIndex, StringComparison.InvariantCultureIgnoreCase);
                string aliasName = node.Arguments.Substring(aliasStartIndex, closeParenIndex - aliasStartIndex);
                sb.Append($" ({aliasName})");
            }

            return sb.ToString();
        }

        private static string ExtractColumnName(SqlParseTreeNode node)
        {
            string pattern = @"Q?COL:\s+(?<colName>.+)";
            Match match = Regex.Match(node.Arguments, pattern);
            if (match.Success)
            {
                string columnName = match.Groups["colName"].Value;
                return columnName.Trim();
            }

            return "<unknown>";
        }

        private static string ExtractConstant(SqlParseTreeNode node)
        {
            string pattern = @"XVAR\((?<dataType>.+?),(?<ownership>.+?),\s*Value=(?<value>.+?)\)";
            Match match = Regex.Match(node.Arguments, pattern);
            if (match.Success)
            {
                string dataType = match.Groups["dataType"].Value;
                string ownership = match.Groups["ownership"].Value;
                string value = match.Groups["value"].Value;

                if (value.StartsWith("Len,Data = "))
                {
                    value += ")";
                    pattern = @"\((?<length>.+?),(?<value>.+?)\)";
                    match = Regex.Match(value, pattern);
                    if (match.Success)
                    {
                        string length = match.Groups["length"].Value;
                        value = match.Groups["value"].Value;
                    }
                }

                return $"({dataType}) {value.Trim()}";
            }

            return "<unknown>";
        }
    }
}
