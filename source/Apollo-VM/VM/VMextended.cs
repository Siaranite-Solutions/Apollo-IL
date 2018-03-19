using System;
using Apollo_IL.StandardLib;
using Apollo_IL.Conversions;

namespace Apollo_IL
{
    public partial class VM
    {
        private int Get32BitParameter(int startingIndex)
        {
            byte[] seperate = new byte[4];
            //First we need to get the bytes to convert.
            for (int i = 0; i < 4; i++)
            {
                // Get each byte
                seperate[i] = ram.memory[startingIndex + i];
            }
            bool[] byte1, byte2, byte3, byte4 = new bool[8];
            byte1 = BitOps.GetBinaryValue(seperate[0]);
            byte2 = BitOps.GetBinaryValue(seperate[1]);
            byte3 = BitOps.GetBinaryValue(seperate[2]);
            byte4 = BitOps.GetBinaryValue(seperate[3]);
            byte1 = Conversions.BooleanArray.JoinBooleans(byte1, byte2);
            byte1 = Conversions.BooleanArray.JoinBooleans(byte1, byte3);
            byte1 = Conversions.BooleanArray.JoinBooleans(byte1, byte4);
            return BitOps.GetIntegerValue(byte1);
        }


        private byte ParseOpcode(byte opcode)
        {
            #region [COMPLETE] MOV (Move, 0x01)
            if (opcode == (byte)0x01)
            {
                // Parameter 1: Destination, must be between 0xF0 and 0xFF
                parameters[1] = ram.memory[(IP + 1)];
                if (!(parameters[1] > 0xEF))
                {
                    //Not a register, throw an error
                    throw new Exception("[CRITICAL ERROR] The value at " + (IP + 1) + "(" + parameters[1] + ") does not refer to a register.");
                }
                if (opMode == AddressMode.RegVal)
                {
                    // Parameter 2: Value, just set the register to the value
                    parameters[2] = Get32BitParameter(IP + 2);
                    SetRegister((byte)parameters[1], parameters[2]);
#if DEBUG
                    Globals.console.WriteLine("MOV " + parameters[1].ToString() + " " + parameters[2].ToString() + "  : " + AL.ToString());
#endif
                }
                else if (opMode == AddressMode.RegReg)
                {
                    // Parameter 2: Register, set the register to the value in the second register
                    parameters[2] = GetRegister(ram.memory[(IP + 2)]);
                    SetRegister((byte)parameters[1], parameters[2]);
                }
                else
                {
                    // Parameter 2: WRONG ADDRESSING MODE
                    throw new Exception("[CRITICAL ERROR] The adressing mode at "+IP+" ("+ AdMode +") is invalid for the MOV (0x01) instruction.");
                }

                PC += 5;
            }
            #endregion
            #region [COMPLETE] MOM/E (Move Memory, 0x3A & 0x3B)
            else if (opcode == (byte)0x3A)
            {
                // Parameter 2: Destination
                parameters[2] = Get32BitParameter(IP + 2);
                // Parameter 1: Source
                if (opMode == AddressMode.RegVal)
                {
                    parameters[1] = GetRegister(ram.memory[IP + 1]);
                    ram.SetByte(parameters[2], (byte)parameters[1]);
                }
                else if (opMode == AddressMode.ValVal)
                {
                    parameters[1] = ram.memory[IP + 1];
                    ram.SetByte(parameters[2], (byte)parameters[1]);
                }
                PC += 5;
            }
            else if (opcode == (byte)0x3B)
            {
                // Parameter 2: Source
                parameters[2] = Get32BitParameter(IP + 2);
                if (opMode == AddressMode.ValVal)
                {
                    // Parameter 1: Destination - Register, put the value in the register into the memory
                    parameters[1] = ram.memory[IP + 1];
                    SetRegister((byte)parameters[1], ram.memory[parameters[2]]);
                    
                }
                else
                {
                    // WRONG ADDRESSING MODE
                    throw new Exception("[CRITICAL ERROR] The adressing mode at " + IP + " (" + AdMode + ") is invalid for the MOM (0x3B) instruction.");
                }

                PC += 5;
            }
            #endregion
            #region [COMPLETE] ADD (Add, 0x04)
            else if (opcode == (byte)0x04)
            {
                if (opMode == AddressMode.RegVal)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) + Get32BitParameter(IP + 2)));
                }
                else if (opMode == AddressMode.RegReg)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) + GetRegister(ram.memory[IP + 2])));
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction ADD does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #region [COMPLETE] JMP (Jump, 0x10)
            else if (opcode == (byte)0x10)
            {
                // Parameter 1: Memory Location, must be between 0xF0 and 0xFF
                parameters[1] = ram.memory[(IP + 1)];
                PC = (byte)parameters[1];
            }
            #endregion
            #region [COMPLETE] CLL (Call, 0x11)
            else if (opcode == (byte)0x11)
            {
                // Parameter 1: Memory Location
                // Add the next position in ram to the call stack
                CallStack.Call((PC + 5));
                // Jump to the location
                parameters[1] = ram.memory[(IP + 1)];
                PC = (byte)parameters[1];
                
                #if DEBUG
                Globals.console.WriteLine("CLL " + parameters[1].ToString());
                #endif
            }
            #endregion
            #region [COMPLETE] RET (Return, 0x12)
            else if (opcode == (byte)0x12)
            {
                // Get the last location from the call stack and jump to it
                PC = (byte)CallStack.Return();
                
                #if DEBUG
                Globals.console.WriteLine("RET " + PC.ToString());
                #endif
            }
            #endregion

            #region [COMPLETE] KEI (Kernel Interrupt, 0x2B)
            else if (opcode == (byte)0x2B)
            {
                // Parameter 1: Kernel Interrupt to call
                parameters[1] = ram.memory[(IP + 1)];
                KernelInterrupts.HandleInterrupt(parameters[1]);
                PC += 5;
            }
            #endregion
            else
            {
                throw new Exception("[ERROR] The instruction at " + IP + " (" + opcode + ") is not supported by this COSIL VM");
            }
            return NewIP = 0;
        }
        /// <summary>
        /// Places content into a register, splitting if necessary
        /// </summary>
        /// <param name="register"></param>
        /// <param name="content"></param>
        private void SetRegister(byte register, int content)
        {
            if (register == (byte)0xF0)
                PC = (byte)content;
            else if (register == (byte)0xF1)
                IP = (byte)content;
            //ignore 0xF2 as it is SP and SP is read only
            else if (register == (byte)0xF3)
            {
                SS = (byte)content;
            }
            else if (register == (byte)0xF4)
            {                
                SetSplit('A', content);
            }
            else if (register == (byte)0xF5)
            {
                AL = (byte)content;
            }
            else if (register == (byte)0xF6)
            {                
                AH = (byte)content;
            }
            else if (register == (byte)0xF7)
            {
                SetSplit('B', content);
            }
            else if (register == (byte)0xF8)
            {   
                BL = (byte)content;
            }
            else if (register == (byte)0xF9)
            {                
                BH = (byte)content;
            }
            else if (register == (byte)0xFA)
            {
                SetSplit('C', content);
            }
            else if (register == (byte)0xFB)
            {
                CL = (byte)content;
            }
            else if (register == (byte)0xFC)
            {   
                CH = (byte)content;
            }
            else if (register == (byte)0xFD)
            {
                X = content;
            }
            else if (register == (byte)0xFE)
            {
                Y = content;
            }
            else
            {
                throw new Exception("ERROR: The register " + register + " is not a register.");
            }
        }
    }
}