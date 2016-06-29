using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public class MemoNodeIcon : NodeIcon
    {
        public MemoNodeIcon()
        {
            Parents = new List<MemoNodeIcon>();
        }

        public SqlMemoNode Node
        {
            get;
            set;
        }

        public List<MemoNodeIcon> Parents
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
