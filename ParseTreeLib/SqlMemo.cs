using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class SqlMemo
    {
        public SqlMemo()
        {
            Groups = new List<SqlMemoGroup>();
        }

        public List<SqlMemoGroup> Groups
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            set;
        }

        public string InnerText
        {
            get;
            set;
        }
    }
}
