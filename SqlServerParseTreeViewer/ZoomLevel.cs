using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerParseTreeViewer
{
    internal class ZoomLevel
    {
        public ZoomLevel(double zoomFraction)
        {
            ZoomFraction = zoomFraction;
        }

        public double ZoomFraction
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return (ZoomFraction * 100).ToString() + "%";
        }
    }
}
