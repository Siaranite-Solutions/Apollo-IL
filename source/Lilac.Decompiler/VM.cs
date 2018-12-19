using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Decompiler
{
    class VMProcessor
    {
        /// <summary>
        /// Enumerable containing the virtual machine's valid Registeristers
        /// </summary>
        public enum VM
        {
            PC,
            IP,
            SP,
            SS,
            A,
            AL,
            AH,
            B,
            BL,
            BH,
            C,
            CL,
            CH,
            X,
            Y
        }

        /// <summary>
        /// Sees if the provided string is a register
        /// </summary>
        /// <param name="Register">String to check</param>
        /// <returns>If the string is a register</returns>
        public static bool CheckRegister(string Register)
        {
            // go through each register to see if the provided string is equal to that,
            //   if so return true, if not return false
            if (Register == "PC")
                return true;
            else if (Register == "IP")
                return true;
            else if (Register == "SP")
                return true;
            else if (Register == "SS")
                return true;
            else if (Register == "A")
                return true;
            else if (Register == "AL")
                return true;
            else if (Register == "AH")
                return true;
            else if (Register == "B")
                return true;
            else if (Register == "BL")
                return true;
            else if (Register == "BH")
                return true;
            else if (Register == "C")
                return true;
            else if (Register == "CL")
                return true;
            else if (Register == "CH")
                return true;
            else if (Register == "X")
                return true;
            else if (Register == "Y")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Parses a string to see if it matches the VMs registers
        /// </summary>
        /// <param name="Register"></param>
        /// <returns>Register bytecode</returns>
        public static byte RetrieveRegister(string Register)
        {
            // go through each Registerister to see if the provided string is equal to that,
            //  return the correct value if equal. Return 0 if not.
            if (Register == "PC")
            {
                return 0xF0;
            }
            else if (Register == "IP")
            {
                return 0xF1;
            }
            else if (Register == "SP")
            {
                return 0xF2;
            }
            else if (Register == "SS")
            {
                return 0xF3;
            }
            else if (Register == "A")
            {
                return 0xF4;
            }
            else if (Register == "AL")
            {
                return 0xF5;
            }
            else if (Register == "AH")
            {
                return 0xF6;
            }
            else if (Register == "B")
            {
                return 0xF7;
            }
            else if (Register == "BL")
            {
                return 0xF8;
            }
            else if (Register == "BH")
            {
                return 0xF9;
            }
            else if (Register == "C")
            {
                return 0xFA;
            }
            else if (Register == "CL")
            {
                return 0xFB;
            }
            else if (Register == "CH")
            {
                return 0xFC;
            }
            else if (Register == "X")
            {
                return 0xFD;
            }
            else if (Register == "Y")
            {
                return 0xFE;
            }
            else
            {
                return 0;
            }
        }
    }
}
