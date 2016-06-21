using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class TreeTextParser
    {
        private const string _beginMarker = "*** ";
        private const string _endMarker = "****";
        private const string _ruleAppliedMarker = "***** Rule applied: ";

        private static char[] _operatorSeparatorChars = new char[] { ' ', '(' };

        public static ReadOnlyCollection<SqlParseTree> Parse(string treeText)
        {
            if (string.IsNullOrEmpty(treeText))
            {
                return new ReadOnlyCollection<SqlParseTree>(new List<SqlParseTree>());
            }

            List<SqlParseTree> trees = new List<SqlParseTree>();
            List<SqlParseTreeNode> nodes = new List<SqlParseTreeNode>();
            SqlParseTree tree = null;
            StringBuilder sb = new StringBuilder();

            using (StringReader reader = new StringReader(treeText))
            {
                string line;
                int lineNumber = 0;
                SqlParseTreeNode previousNode = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(_endMarker) &&
                        line.StartsWith(_ruleAppliedMarker) == false &&
                        tree != null)
                    {
                        // End of current tree reached.
                        // TODO: determine offset at end
                        tree.EndOffset = 0;
                        tree.InnerTreeText = sb.ToString();
                    }
                    else if (line.StartsWith(_beginMarker) ||
                        line.StartsWith(_ruleAppliedMarker))
                    {
                        if (tree != null)
                        {
                            tree.RootNode = ConvertNodeListToTree(nodes);
                            tree.InnerTreeText = sb.ToString();
                            trees.Add(tree);
                            nodes = new List<SqlParseTreeNode>();
                            sb = new StringBuilder();
                        }
                        tree = new SqlParseTree();
                        tree.OuterTreeText = treeText;
                        tree.BeginOffset = 0;
                        tree.TreeDescription = ExtractTreeDescription(line);
                    }
                    else if (line.StartsWith(" ") == false)
                    {
                        //throw new ApplicationException(string.Format("Unknown text {0} in parse tree text (no indentation?)", line));
                    }
                    else
                    {
                        if ((line.Trim().Length > 0 && line.Trim().StartsWith("=") && previousNode != null) ||
                            (previousNode != null && previousNode.OperationName == "Exchange" && previousNode.Arguments == "Partition"))
                        {
                            // Continuation of previous line

                            previousNode.Arguments += line;
                            sb.AppendLine(line);
                        }
                        else
                        {
                            SqlParseTreeNode node = ParseSingleNode(line);
                            node.SequenceNumber = lineNumber;
                            nodes.Add(node);
                            sb.AppendLine(line);
                            previousNode = node;

                            lineNumber++;
                        }
                    }
                }
            }

            if (tree != null)
            {
                tree.RootNode = ConvertNodeListToTree(nodes);
                tree.EndOffset = 0;
                tree.InnerTreeText = sb.ToString();
                trees.Add(tree);
            }

            return new ReadOnlyCollection<SqlParseTree>(trees);
        }

        private static string ExtractTreeDescription(string parseLine)
        {
            if (parseLine.StartsWith(_ruleAppliedMarker))
            {
                return parseLine.Substring(_ruleAppliedMarker.Length).Trim();
            }
            return parseLine.Trim('*').Trim(' ').TrimEnd(':');
        }

        private static SqlParseTreeNode ParseSingleNode(string parseLine)
        {
            SqlParseTreeNode node = new SqlParseTreeNode();

            // Determine level by how many tabs prefix the text.
            int spaces = CountLeadingSpaces(parseLine);
            //node.Level = spaces / 4;
            node.Level = spaces;

            // Determine the operation name
            parseLine = parseLine.TrimStart();
            parseLine = FixupLine(parseLine);
            int spaceIndex = parseLine.IndexOfAny(_operatorSeparatorChars);
            if (spaceIndex < 0)
            {
                node.OperationName = parseLine;
                parseLine = string.Empty;
            }
            else
            {
                node.OperationName = parseLine.Substring(0, spaceIndex);
                parseLine = parseLine.Substring(spaceIndex).TrimStart();
            }

            OperationType operationType;
            if (Enum.TryParse(node.OperationName, out operationType))
            {
                node.Operation = operationType;
            }

            // Arguments is the remainder
            node.Arguments = parseLine;

            return node;
        }

        private static string FixupLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return line;
            }

            string orderByIdentifier = "LogOp_OrderBy";
            if (line.StartsWith(orderByIdentifier))
            {
                // Insert a space after the identifier
                line = orderByIdentifier + " " + line.Substring(orderByIdentifier.Length);
            }

            return line;
        }

        private static int CountLeadingSpaces(string text, int tabValue = 4)
        {
            int spaces = 0;
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    spaces++;
                }
                else if (c == '\t')
                {
                    spaces += tabValue;
                }
                else
                {
                    break;
                }
            }

            return spaces;
        }

        private static SqlParseTreeNode ConvertNodeListToTree(List<SqlParseTreeNode> nodeList)
        {
            if (nodeList == null ||
                nodeList.Count == 0)
            {
                return new SqlParseTreeNode();
            }

            SqlParseTreeNode rootNode = nodeList[0];
            SqlParseTreeNode previousNode = rootNode;

            for (int nodeIndex = 1; nodeIndex < nodeList.Count; nodeIndex++)
            {
                SqlParseTreeNode node = nodeList[nodeIndex];
                if (node.Level > previousNode.Level)
                {
                    node.Parent = previousNode;
                }
                else if (node.Level == previousNode.Level)
                {
                    // If the previous node is currently considered to be the "root", need to split
                    // the root so that we don't wind up with muliple roots.
                    if (previousNode == rootNode)
                    {
                        rootNode = new SqlParseTreeNode();
                        rootNode.OperationName = string.Empty;
                        previousNode.Parent = rootNode;
                    }

                    node.Parent = previousNode.Parent;
                }
                else
                {
                    SqlParseTreeNode siblingSearchNode = previousNode.Parent;
                    while (siblingSearchNode != null &&
                           siblingSearchNode.Level > node.Level)
                    {
                        siblingSearchNode = siblingSearchNode.Parent;
                    }

                    if (siblingSearchNode != null)
                    {
                        node.Parent = siblingSearchNode.Parent;
                    }
                }
                if (node.Parent != null)
                {
                    node.Parent.Children.Add(node);
                }

                previousNode = node;
            }

            return rootNode;
        }
    }
}
