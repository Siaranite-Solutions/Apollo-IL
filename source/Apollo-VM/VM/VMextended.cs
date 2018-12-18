using System;
using Apollo_IL.StandardLib;
using Apollo_IL.Conversions;

namespace Apollo_IL
{
    public partial class VM
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingIndex"></param>
        /// <returns>Integet value</returns>
        private int Get32BitParameter(int startingIndex)
        {
            byte[] seperate = new byte[4];
            //First we need to get the bytes to convert.
            for (int i = 0; i < 4; i++)
            {
                // Get each byte
                seperate[i] = ram.memory[startingIndex + i];
            }
            bool[] b1 = new bool[8];
            bool[] b2 = new bool[8];
            bool[] b3 = new bool[8];
            bool[] b4 = new bool[8];
            b1 = BitOps.GetBinaryValue(seperate[0]);
            b2 = BitOps.GetBinaryValue(seperate[1]);
            b3 = BitOps.GetBinaryValue(seperate[2]);
            b4 = BitOps.GetBinaryValue(seperate[3]);
            b1 = Conversions.BooleanArray.JoinBooleans(b1, b2);
            b1 = Conversions.BooleanArray.JoinBooleans(b1, b3);
            b1 = Conversions.BooleanArray.JoinBooleans(b1, b4);
            return BitOps.GetIntegerValue(b1);
        }

        /// <summary>
        /// Places content into a register, splitting if necessary
        /// </summary>
        /// <param name="Register"></param>
        /// <param name="Content"></param>
        private void SetRegister(byte Register, int Content)
        {
            if (Register == (byte)0xF0)
                PC = (byte)Content;
            else if (Register == (byte)0xF1)
                IP = (byte)Content;
            //0xF2 (Stack Pointer) is read only
            else if (Register == (byte)0xF3)
                SS = (byte)Content;
            else if (Register == (byte)0xF4)
                SetSplit('A', Content);
            else if (Register == (byte)0xF5)
                AL = (byte)Content;
            else if (Register == (byte)0xF6)
                AH = (byte)Content;
            else if (Register == (byte)0xF7)
                SetSplit('B', Content);
            else if (Register == (byte)0xF8)
                BL = (byte)Content;
            else if (Register == (byte)0xF9)
                BH = (byte)Content;
            else if (Register == (byte)0xFA)
                SetSplit('C', Content);
            else if (Register == (byte)0xFB)
                CL = (byte)Content;
            else if (Register == (byte)0xFC)
                CH = (byte)Content;
            else if (Register == (byte)0xFD)
                X = Content;
            else if (Register == (byte)0xFE)
                Y = Content;
            else
                throw new Exception("ERROR: The register " + Register + " is not a register.");
        }

        /// <summary>
        /// Returns an integer value stored in a register
        /// </summary>
        /// <param name="Reg"></param>
        /// <returns></returns>
        private int GetRegister(byte Register)
        {
            if (Register == (byte)0xF0)
                return (int)PC;
            else if (Register == (byte)0xF1)
                return (int)IP;
            else if (Register == (byte)0xF2)
                return (int)SP;
            else if (Register == (byte)0xF3)
                return (int)SS;
            else if (Register == (byte)0xF4)
                return GetSplit('A');
            else if (Register == (byte)0xF5)
                return AL;
            else if (Register == (byte)0xF6)
                return AH;
            else if (Register == (byte)0xF7)
                return GetSplit('B');
            else if (Register == (byte)0xF8)
                return BL;
            else if (Register == (byte)0xF9)
                return BH;
            else if (Register == (byte)0xFA)
                return GetSplit('C');
            else if (Register == (byte)0xFB)
                return CH;
            else if (Register == (byte)0xFC)
                return CL;
            else if (Register == (byte)0xFD)
                return X;
            else if (Register == (byte)0xFE)
                return Y;
            else
                return 0;
        }
    }
}