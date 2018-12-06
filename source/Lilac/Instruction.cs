using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Compiler
{
    class Instruction
    {

        public byte[] InstructionBytecode;

        /// <summary>
        /// Create a full 48 bit COSIL instruction
        /// </summary>
        /// <param name="Instruction">Opcode for this instruction</param>
        /// <param name="Mode">Addressing mode required</param>
        /// <param name="ins1">Parameter one (8 bits)</param>
        /// <param name="ins2">Parameter two (32 bits)</param>
        /// <returns>A full 6 byte instruction in an array</returns>
        public Instruction(byte Instruction, AddressMode Mode, byte ins1, int ins2)
        {
            // create a blank byte array ready for the COSIL instruction format of 6 bits, 2 bits, 8 bits, 32 bits
            InstructionBytecode = new byte[6];
            // create a blank binary array ready for getting the first byte of the instruction
            bool[] eightBit = new bool[8];
            // get the binary value of just the instruction
            eightBit = BitOperations.GetBinaryValue(Instruction);
            // set the ad mode for the instruction byte
            if (Mode == AddressMode.RegisterRegister)
            {
                eightBit[6] = false;
                eightBit[7] = false;
            }
            else if (Mode == AddressMode.RegisterValue)
            {
                eightBit[6] = true;
                eightBit[7] = false;
            }
            else if (Mode == AddressMode.ValueRegister)
            {
                eightBit[6] = false;
                eightBit[7] = true;
            }
            else if (Mode == AddressMode.ValueValue)
            {
                eightBit[6] = true;
                eightBit[7] = true;
            }
            // get the instruction back into an eight bit C# byte
            InstructionBytecode[0] = (byte)BitOperations.GetIntegerValue(eightBit);
            // now set the first parameter byte of eight bits
            InstructionBytecode[1] = ins1;
            // now we need to set the remaining 32 bit integer
            byte[] temp = BitConverter.GetBytes(ins2);
            InstructionBytecode[2] = temp[0];
            InstructionBytecode[3] = temp[1];
            InstructionBytecode[4] = temp[2];
            InstructionBytecode[5] = temp[3];
            // finally return the finished instruction
            
        }

        /// <summary>
        /// Get the opcode byte of an instruction mnemonic
        /// </summary>
        /// <param name="ins">The instruction mnemonic to get an opcode for</param>
        /// <returns>Instruction opcode byte, returns 0 if not a valid instruction</returns>
        public static byte Get(string ins)
        {
            // go through each instruction seeing if the string is equal to that,
            //  if so return the opcode for that instruction, if not return 0
            #region "Register Operations"
            if (ins == "MOV")
                return 0x01;
            else if (ins == "MOM")
                return 0x3A;
            else if (ins == "MOE")
                return 0x3B;
            else if (ins == "SWP")
                return 0x02;
            else if (ins == "TEQ")
                return 0x1B;
            else if (ins == "TNE")
                return 0x1C;
            else if (ins == "TLT")
                return 0x1D;
            else if (ins == "TGT")
                return 0x1E;
            #endregion

            #region "Arithmetic"
            else if (ins == "ADD")
                return 0x04;
            else if (ins == "SUB")
                return 0x05;
            else if (ins == "INC")
                return 0x08;
            else if (ins == "DEC")
                return 0x09;
            else if (ins == "MUL")
                return 0x30;
            else if (ins == "DIV")
                return 0x31;
            #endregion

            #region "Bitwise Operations"
            else if (ins == "SHL")
                return 0x06;
            else if (ins == "SHR")
                return 0x07;
            else if (ins == "ROR")
                return 0x0F;
            else if (ins == "ROL")
                return 0x0E;
            else if (ins == "AND")
                return 0x0A;
            else if (ins == "BOR")
                return 0x0B;
            else if (ins == "XOR")
                return 0x0C;
            else if (ins == "NOT")
                return 0x0D;
            #endregion
            #region "Flow Operations"
            else if (ins == "JMP")
                return 0x10;
            else if (ins == "CLL")
                return 0x11;
            else if (ins == "RET")
                return 0x12;
            else if (ins == "JMT")
                return 0x13;
            else if (ins == "JMF")
                return 0x14;
            else if (ins == "CLT")
                return 0x17;
            else if (ins == "CLF")
                return 0x18;
            #endregion

            #region "Stack Control"
            else if (ins == "PSH")
                return 0x20;
            else if (ins == "POP")
                return 0x21;
            #endregion

            #region "I/O"
            else if (ins == "INB")
                return 0x24;
            else if (ins == "INW")
                return 0x25;
            else if (ins == "IND")
                return 0x26;
            else if (ins == "OUB")
                return 0x27;
            else if (ins == "OUW")
                return 0x28;
            else if (ins == "OUD")
                return 0x29;
            #endregion

            #region "Interrupts"
            else if (ins == "SWI")
                return 0x2A;
            else if (ins == "KEI")
                return 0x2B;
            #endregion

            else
                return 0;

        }
    }
}
