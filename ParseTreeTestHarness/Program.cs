using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bkh.ParseTreeLib;

namespace ParseTreeTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string treeText = System.IO.File.ReadAllText(@"C:\temp\SampleInputTree.txt");
                List<SqlParseTree> tree = new List<SqlParseTree>(TreeTextParser.Parse(treeText));
            }
            catch (Exception ex)
            {
                Console.WriteLine("{ 0}", ex);
            }
        }
    }
}
