using System;
using Apollo_IL.StandardLib;
using Apollo_IL.Conversions;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Apollo_IL
{
    public partial class VM
    {
        /// <summary>
        /// Retrieves the last two bits from a single byte (8 bits)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private int GetLastTwo(byte b)
        {
            byte c = 0;
            for (int i = 7; i > 5; i--)
            {
                sixbits[i] = GetBit(b, i);
            }
            return getIntegerValue(sixbits);
        }
        /// <summary>
        /// Retrieves a specific bit from a single byte (bit array [8])
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bitNumber"></param>
        /// <returns></returns>
        public static bool getBit(byte b, int bitNumber)
        {
            return ((int)b & (1 << bitNumber)) != 0;
        }
        public static int getIntegerValue(bool[] bits)
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
        /// Returns a given byte as a bool array
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool[] getBinaryValue(byte b)
        {
            bool[] ret = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                ret[i] = getBit(b, i);
            }
            return ret;
        }
        
        public static int combineBytes(byte one, byte two)
        {
            bool[] sixteenBit = new bool[16];
            bool[] binaryOne = getBinaryValue(one);
            bool[] binaryTwo = getBinaryValue(two);
            sixteenBit = Conversions.BooleanArray.JoinBooleans(binaryOne, binaryTwo);
            return getIntegerValue(sixteenBit);
        }
    } 
}