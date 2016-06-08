using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    [DataContract]
    class OperatorColor
    {
        [DataMember]
        public string OperatorName
        {
            get;
            set;
        }

        [DataMember]
        public Color DisplayColor
        {
            get;
            set;
        }

        public override string ToString()
        {
            return OperatorName;
        }
    }
}
