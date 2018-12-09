using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Compiler
{
    class CharValue
    {
        /// <summary>
        /// Check if a string contains a valid Char value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool Check(string s)
        {
            if (s.StartsWith("'") && s.EndsWith("'"))
            {
                if (s.Contains("\\") && s.Length == 4)
                    return true;
                else if (s.Length == 3)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// Parses a string to retrieve the contained Char value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte Parse(string s)
        {
            s = s.Replace("'", "");
            if (s.Contains("\\"))
            {
                s = s.Replace("\\", "");
                if (s == "n")
                    return (byte)'\n';
                else if (s == "r")
                    return (byte)'\r';
                else if (s == "a")
                    return (byte)'\a';
                else if (s == "b")
                    return (byte)'\b';
                else if (s == "t")
                    return (byte)'\t';
                else if (s == "v")
                    return (byte)'\v';
                else if (s == "\'")
                    return (byte)'\'';
                else if (s == "\"")
                    return (byte)'\"';
                else if (s == "\\")
                    return (byte)'\\';
                else
                    return (byte)s[0];
            }
            else
            {
                return (byte)s[0];
            }
        }
    }
}
