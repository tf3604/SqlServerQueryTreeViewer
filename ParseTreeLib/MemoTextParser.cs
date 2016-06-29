using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class MemoTextParser
    {
        private const string _initialMemoHeader = "--- Initial Memo Structure ---";
        private const string _finalMemoHeader = "--- Final Memo Structure ---";
        private const string _memoEndMarker = "------------------------------";

        public static ReadOnlyCollection<SqlMemo> Parse(string memoText)
        {
            if (string.IsNullOrEmpty(memoText))
            {
                return new ReadOnlyCollection<SqlMemo>(new List<SqlMemo>());
            }

            List<SqlMemo> memos = new List<SqlMemo>();
            SqlMemo memo = null;
            StringBuilder sb = new StringBuilder();

            using (StringReader reader = new StringReader(memoText))
            {
                string line;
                int lineNumber = 0;
                SqlMemoGroup group = null;
                Dictionary<SqlMemoNode, List<SqlMemoNodeIdentifier>> parentMappings = null;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;

                    if (line.StartsWith(_memoEndMarker))
                    {
                    }
                    else if (line.StartsWith(_initialMemoHeader) ||
                        line.StartsWith(_finalMemoHeader))
                    {
                        if (memo != null)
                        {
                            LinkNodes(memo, parentMappings);
                            memo.InnerText = sb.ToString();
                            memos.Add(memo);

                            memo = null;
                            sb = new StringBuilder();
                        }

                        memo = new SqlMemo();
                        parentMappings = new Dictionary<SqlMemoNode, List<SqlMemoNodeIdentifier>>();

                        if (line.StartsWith(_initialMemoHeader))
                        {
                            memo.Description = "Initial Memo";
                        }
                        else if (line.StartsWith(_finalMemoHeader))
                        {
                            memo.Description = "Final Memo";
                        }
                    }
                    else if (string.IsNullOrEmpty(line.Trim()))
                    {
                        // Skip
                    }
                    else if (line.StartsWith(" "))
                    {
                        if (group != null)
                        {
                            sb.AppendLine(line);

                            SqlMemoNode node = new SqlMemoNode();
                            int groupNumber;
                            int operationNumber;
                            ReadGroupNumber(ref line, group.GroupNumber, out groupNumber, out operationNumber);
                            node.Identifier.GroupNumber = groupNumber;
                            node.Identifier.OperationNumber = operationNumber;
                            node.OperationName = ReadOperatorName(ref line);

                            parentMappings.Add(node, new List<SqlMemoNodeIdentifier>());
                            while (ReadGroupNumber(ref line, null, out groupNumber, out operationNumber))
                            {
                                SqlMemoNodeIdentifier parentIdentifier = new SqlMemoNodeIdentifier();
                                parentIdentifier.GroupNumber = groupNumber;
                                parentIdentifier.OperationNumber = operationNumber;
                                parentMappings[node].Add(parentIdentifier);
                            }

                            node.Arguments = line.Trim();
                            group.Operations.Add(node);
                        }
                    }
                    else
                    {
                        if (memo != null)
                        {
                            string originalLine = line;
                            bool isRoot = false;
                            string rootMarker = "Root ";
                            if (line.StartsWith(rootMarker))
                            {
                                isRoot = true;
                                line = line.Substring(rootMarker.Length);
                            }

                            line = line.Trim();
                            string groupMarkerPattern = @"^Group (?<groupNumber>\d+):(?<arguments>.*)$";
                            Match match = Regex.Match(line, groupMarkerPattern);
                            if (match.Success)
                            {
                                string groupNumberString = match.Groups["groupNumber"].Value;
                                string arguments = match.Groups["arguments"].Value;
                                int groupNumber;
                                int.TryParse(groupNumberString, out groupNumber);

                                group = new SqlMemoGroup();
                                group.GroupNumber = groupNumber;
                                group.IsRoot = isRoot;
                                group.Arguments = arguments;

                                memo.Groups.Add(group);
                                sb.AppendLine(originalLine);
                            }
                        }
                    }
                }

                if (memo != null)
                {
                    LinkNodes(memo, parentMappings);
                    memo.InnerText = sb.ToString();
                    memos.Add(memo);
                }
            }

            return new ReadOnlyCollection<SqlMemo>(memos);
        }

        private static void LinkNodes(SqlMemo memo, Dictionary<SqlMemoNode, List<SqlMemoNodeIdentifier>> parentMappings)
        {
            foreach (SqlMemoNode node in parentMappings.Keys)
            {
                foreach (SqlMemoNodeIdentifier parent in parentMappings[node])
                {
                    SqlMemoNode matchingNode = memo.Groups.SelectMany(g => g.Operations).FirstOrDefault(n => n.Identifier == parent);
                    if (matchingNode != null)
                    {
                        node.Parents.Add(matchingNode);
                    }
                }
            }
        }

        private static bool ReadGroupNumber(ref string memoString, int? defaultGroupNumber, out int groupNumberValue, out int operationNumberValue)
        {
            string groupNumber = string.Empty;
            string operationNumber = string.Empty;

            while (memoString.StartsWith(" "))
            {
                memoString = memoString.Substring(1);
            }

            bool includesDecimal = false;
            if (defaultGroupNumber != null)
            {
                includesDecimal = true;
            }

            bool isValueRead = false;

            while (string.IsNullOrEmpty(memoString) == false &&
                (char.IsDigit(memoString[0]) ||
                (memoString[0] == '.' && includesDecimal == false)))
            {
                if (memoString[0] == '.')
                {
                    includesDecimal = true;
                }
                else if (includesDecimal == false)
                {
                    groupNumber += memoString[0];
                }
                else
                {
                    operationNumber += memoString[0];
                }
                memoString = memoString.Substring(1);
                isValueRead = true;
            }

            if (defaultGroupNumber != null)
            {
                groupNumberValue = defaultGroupNumber.Value;
            }
            else
            {
                int.TryParse(groupNumber, out groupNumberValue);
            }

            int.TryParse(operationNumber, out operationNumberValue);
            return isValueRead;
        }

        private static string ReadOperatorName(ref string memoString)
        {
            string operatorName = string.Empty;
            while (memoString.StartsWith(" "))
            {
                memoString = memoString.Substring(1);
            }

            while (string.IsNullOrEmpty(memoString) == false &&
                memoString.StartsWith(" ") == false)
            {
                operatorName += memoString[0];
                memoString = memoString.Substring(1);
            }

            return operatorName;
        }
    }
}
