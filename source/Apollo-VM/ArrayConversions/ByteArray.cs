using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Apollo_IL.Conversions
{
    /// <summary>
    /// Contains the byte[] methods for joining two boolean arrays
    /// </summary>
    public static class ByteArray
    {
        /// <summary>
        /// Converts each byte in an array into a char and returns them in a string
        /// </summary>
        /// <param name="content"></param>
        /// <returns>A string made from the chars in each byte in the array</returns>
        public static string ToString(byte[] content)
        {
            string ret = "";
            for (int i = 0; i < content.Length; i++)
            {
                ret += (char)content[i];
            }
            return ret;
        }
        /// <summary>
        /// Adds two byte arrays (byte_1, byte_2) together and returns the new byte array
        /// </summary>
        /// <param name="byte_1"></param>
        /// <param name="byte_2"></param>
        /// <returns>Array two joined at the end of Array one</returns>
        public static byte[] JoinOn(byte[] byte_1, byte[] bool_2)
        {
            byte[] ret = new byte[(byte_1.Length + bool_2.Length)];
            for (int i = 0; i < byte_1.Length; i++)
            {
                ret[i] = byte_1[i];
            }
            for (int i = 0; i < bool_2.Length; i++)
            {
                ret[(i + byte_1.Length)] = bool_2[i];
            }
            return ret;
        }
    }
}