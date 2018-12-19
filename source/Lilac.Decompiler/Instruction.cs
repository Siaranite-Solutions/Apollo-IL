using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Decompiler
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
            InstructionBytecode[0] = BitOperations.GetByteValue(eightBit);
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

        
    }
}
