using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public static class Utilities
    {
        public static string FixupLineEndings(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Look for \n without preceding \r; replace with \r\n
            char carriageReturn = '\r';
            char lineFeed = '\n';

            char previousChar = '\0';
            StringBuilder sb = new StringBuilder();
            foreach (char currentChar in text)
            {
                if (currentChar == lineFeed &&
                    previousChar != carriageReturn)
                {
                    sb.Append(carriageReturn);
                }

                sb.Append(currentChar);
                previousChar = currentChar;
            }

            return sb.ToString();
        }

        public static double DistanceTo(this Point thisPoint, Point point)
        {
            return Math.Sqrt(Math.Pow(thisPoint.X - point.X, 2.0) + Math.Pow(thisPoint.Y - point.Y, 2.0));
        }

        /// <summary>
        /// Generic method to create a clone of an object.
        /// </summary>
        /// <typeparam name="T">Type of the object to be cloned.  This type must be serializable using the DataContractSerializer; otherwise,
        /// either a run-time error will result or an incomplete clone will be generated.</typeparam>
        /// <param name="original">The original object to be cloned.</param>
        /// <returns>A clone of the original object.</returns>
        public static T CreateClone<T>(T original)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(ms, original);
                ms.Position = 0;
                T clone = (T)serializer.ReadObject(ms);
                return clone;
            }
        }
    }
}
