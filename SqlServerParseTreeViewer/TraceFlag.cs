using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    [DataContract]
    class TraceFlag
    {
        private static List<TraceFlag> _defaultTraceFlagList = null;

        public TraceFlag(int traceFlagNumber, string description, int order)
        {
            TraceFlagNumber = traceFlagNumber;
            Description = description;
            Order = order;
            Enabled = false;
        }

        [DataMember]
        public int TraceFlagNumber
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        public bool Enabled
        {
            get;
            set;
        }

        [DataMember]
        public int Order
        {
            get;
            set;
        }

        [DataMember]
        public int? ParentTraceFlag
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            TraceFlag that = obj as TraceFlag;
            if (that == null)
            {
                return false;
            }

            return this.TraceFlagNumber == that.TraceFlagNumber;
        }

        public override int GetHashCode()
        {
            return TraceFlagNumber.GetHashCode();
        }

        public static ReadOnlyCollection<TraceFlag> DefaultTraceFlagList
        {
            get
            {
                if (_defaultTraceFlagList == null)
                {
                    _defaultTraceFlagList = new List<TraceFlag>()
                    {
                        new TraceFlag(3604, "Output to client", 1),
                        new TraceFlag(8605, "Show initial query tree", 2),
                        new TraceFlag(8606, "Show intermediate query trees", 3),
                        new TraceFlag(8607, "Show output tree", 4),
                        new TraceFlag(8609, "Show task information", 5),
                        new TraceFlag(8619, "Show applied rules", 6),
                        new TraceFlag(8620, "Show applied rules with memo information (requires TF8619)", 7) { ParentTraceFlag = 8619 },
                        new TraceFlag(8675, "Show optimization phases and search times", 8),
                        new TraceFlag(2372, "Show memory usage during optimization", 9),
                        new TraceFlag(2373, "Show memory usage during property derivation", 10)
                    };
                }

                return new ReadOnlyCollection<TraceFlag>(_defaultTraceFlagList);
            }
        }
    }
}
