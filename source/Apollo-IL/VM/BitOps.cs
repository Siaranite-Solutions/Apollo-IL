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
        /// <summary>
        /// Retrieves the integer value of an array of booleans
        /// </summary>
        /// <param name="bits"></param>
        /// <returns>Integer value of the specified array</returns>
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
        /// <summary>
        /// Combines the specified bytes into a single integer value and returns it
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>Integer of the two bytes combined</returns>
        public static int CombineBytes(byte one, byte two)
        {
            bool[] sixteenBit = new bool[16];
            bool[] binaryOne = getBinaryValue(one);
            bool[] binaryTwo = getBinaryValue(two);
            sixteenBit = Conversions.BooleanArray.JoinBooleans(binaryOne, binaryTwo);
            return getIntegerValue(sixteenBit);
        }
        /// <summary>
        /// Stores content into registers, splitting the content into the two register halves if needed
        /// </summary>
        /// <param name="register"></param>
        /// <param name="content"></param>
        public void SetSplit(char register, int content)
        {
            byte lower;
            byte higher;
            if (content > 255)
            {
                lower = (byte) 255;
                higher = (byte) (content - 255);
            }
            else
            {
                lower= (byte) content;
                if (register == 'A')
                {
                    AL = lower;
                }
                else if (register == 'B')
                {
                    BL = lower;
                }
                else if (register == 'C')
                {
                    CL = lower;
                }
            }
        }
        /// <summary>
        /// Retrieves the data stored in each register half (AL/AH, BL/BH, CL/CH), returning the integer value
        /// If the specified register isn't A/B/C, throw a new exception.
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Two halves of the specified register combined into one integer</returns>
        public int GetSplit(char register)
        {
            if (register == 'A')
            {
                return CombineBytes(AL, AH);
            }
            else if (register == 'B')
            {
                return CombineBytes(BL, BH);
            }
            else if (register == 'C')
            {
                return CombineBytes(CL, CH);
            }
            throw new Exception("There was an internal error and the VM has closed to protect your data. Please report this to the application developer");
        }
    } 
}