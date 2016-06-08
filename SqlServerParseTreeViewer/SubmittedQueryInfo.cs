using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    [DataContract]
    internal class SubmittedQueryInfo
    {
        public SubmittedQueryInfo(string queryText)
        {
            if (string.IsNullOrEmpty(queryText))
            {
                throw new ArgumentNullException(nameof(queryText));
            }

            QueryText = queryText;
            AssociatedTime = DateTime.Now;
        }

        [DataMember]
        public string QueryText
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime AssociatedTime
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return AssociatedTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}
