using System;
using Apollo_IL.StandardLib;
using Apollo_IL.Conversions;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Apollo_IL
{
    public static class BitOps
    {
        /// <summary>
        /// Retrieves the integer value of an array of booleans
        /// </summary>
        /// <param name="bits"></param>
        /// <returns>Integer value of the specified array</returns>
        public static int GetIntegerValue(bool[] bits)
        {
            int ans = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true)
                {
                    ans += (int)Math.Pow(2, i);
                }
            }
            return ans;
        }
        /// <summary>
		/// Retrieves a bit from a specified byte
		/// false if 0, true if 1
		/// </summary>
		/// <param name="b"></param>
		/// <param name="bitNumber"></param>
		/// <returns></returns>
		public static bool GetBit(byte b, int bitNumber)
        {
            return ((int)b & (1 << bitNumber)) != 0;
        }
        /// <summary>
        /// Returns a given byte as a bool array
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool[] GetBinaryValue(byte b)
        {
            bool[] ret = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                ret[i] = GetBit(b, i);
            }
            return ret;
        }
        /// <summary>
        /// Combines the specified bytes into a single integer value and returns it
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>Integer of the two bytes combined</returns>
        public static int CombineBytes(byte one, byte two)
        {
            bool[] sixteenBit = new bool[16];
            bool[] binaryOne = GetBinaryValue(one);
            bool[] binaryTwo = GetBinaryValue(two);
            sixteenBit = Conversions.BooleanArray.JoinBooleans(binaryOne, binaryTwo);
            return GetIntegerValue(sixteenBit);
        }
    } 
}