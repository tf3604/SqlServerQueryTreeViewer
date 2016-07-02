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
