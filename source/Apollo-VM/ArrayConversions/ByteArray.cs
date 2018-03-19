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
            string string_from_byte = "";
            for (int a = 0; a < content.Length; a++)
            {
                string_from_byte += (char)content[a];
            }
            return string_from_byte;
        }
        /// <summary>
        /// Adds two byte arrays (byte_1, byte_2) together and returns the new byte array
        /// </summary>
        /// <param name="byte_1"></param>
        /// <param name="byte_2"></param>
        /// <returns>Array two joined at the end of Array one</returns>
        public static byte[] JoinBytes(byte[] byte_1, byte[] byte_2)
        {
            byte[] returned_byte = new byte[byte_1.Length + byte_2.Length];
            for (int a = 0; a < byte_1.Length; a++)
            {
                returned_byte[a] = byte_1[a];
            }
            for (int b = 0; b < byte_2.Length; b++)
            {
                returned_byte[(b + byte_1.Length)] = byte_2[b];
            }
            return returned_byte;
        }
    }
}