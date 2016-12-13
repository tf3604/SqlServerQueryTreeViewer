//  Copyright(c) 2016 Brian Hansen.

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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerQueryTreeViewer
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
