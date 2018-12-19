using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Decompiler
{
    class BitOperations
    {

        /// <summary>
        /// Stores the first six bits in a byte
        /// </summary>
        private static bool[] sixbits = new bool[6];

        /// <summary>
        /// Retrieves the integer value of first six bits in a byte
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte GetFirstSix(byte b)
        {
            sixbits = new bool[6];
            for (int i = 0; i < 6; i++)
            {
                sixbits[i] = GetBit(b, i);
            }
            return GetByteValue(sixbits);
        }

        /// <summary>
        /// Retrieves the integer value of first six bits in a byte
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte GetAddressMode(byte b)
        {
            twobits = new bool[2];
            byte c = 0;
            for (int i = 7; i > 5; i--)
            {
                twobits[c] = GetBit(b, i);
                c++;
            }
            return GetByteValue(twobits);
        }

        /// <summary>
        /// Stores the last two bits in a byte (byte - sixbits)
        /// </summary>
        private static bool[] twobits = new bool[2];

        /// <summary>
        /// Get a specific bit from a byte
        /// </summary>
        /// <param name="b">The byte to get the bit from</param>
        /// <param name="bitNumber">The bit to get from the byte</param>
        /// <returns>The bit requested</returns>
        public static bool GetBit(byte b, int bitNumber)
        {
            return ((int)b & (1 << bitNumber)) != 0;
        }
        /// <summary>
        /// Get the byte value of a binary array
        /// </summary>
        /// <param name="bits">The binary number</param>
        /// <returns>The integer number of the binary</returns>
        public static byte GetByteValue(bool[] bits)
        {
            byte ans = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true)
                {
                    ans += (byte) Math.Pow(2, i);
                }
            }
            return ans;
        }
        /// <summary>
        /// Get the binary number of the byte
        /// </summary>
        /// <param name="b">The byte to convert into binary</param>
        /// <returns>The binary value of the byte</returns>
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
        /// Combines the bytes in a binary way
        /// </summary>
        /// <param name="one">The byte to start with</param>
        /// <param name="two">The byte to be combined</param>
        /// <returns>Byte one's binary value with byte two's binary value appended on</returns>
        public static int CombineBytes(byte one, byte two)
        {
            bool[] sixteenBit = new bool[16];
            bool[] binaryOne = GetBinaryValue(one);
            bool[] binaryTwo = GetBinaryValue(two);
            sixteenBit = JoinOn(binaryOne, binaryTwo);
            return GetByteValue(sixteenBit);
        }
        /// <summary>
        /// Adds binary array two to the end of byte array one.
        /// </summary>
        /// <param name="one">Array to start with</param>
        /// <param name="two">Array to append</param>
        /// <returns>Array one with array two appended on</returns>
        public static bool[] JoinOn(bool[] one, bool[] two)
        {
            bool[] ret = new bool[(one.Length + two.Length)];
            for (int i = 0; i < one.Length; i++)
            {
                ret[i] = one[i];
            }
            for (int i = 0; i < two.Length; i++)
            {
                ret[(i + one.Length)] = two[i];
            }
            return ret;
        }
    }
}
