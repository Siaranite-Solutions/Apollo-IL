using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo_IL
{
    public partial class VM
    {
        /// <summary>
        /// Parses a byte as an instruction
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns>Byte of opcode</returns>
        private byte ParseOpcode(byte opcode)
        {
            #region Register Operations
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
                    throw new Exception("[CRITICAL ERROR] The adressing mode at " + IP + " (" + AdMode + ") is invalid for the MOV (0x01) instruction.");
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
            #endregion

            #region Arithmetic
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
            #region [Unfinished] SUB (Subtract, 0x05)
            else if (opcode == (byte)0x05)
            {
                if (opMode == AddressMode.RegVal)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) - Get32BitParameter(IP + 2)));
                }
                else if (opMode == AddressMode.RegReg)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) - GetRegister(ram.memory[IP + 2])));
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction SUB does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #region [Unfinished] MUL (Multiply, 0x30)
            else if (opcode == (byte)0x30)
            {
                if (opMode == AddressMode.RegVal)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) * Get32BitParameter(IP + 2)));
                }
                else if (opMode == AddressMode.RegReg)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) * GetRegister(ram.memory[IP + 2])));
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction MUL does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #region [Unfinished] DIV (Divide, 0x31)
            else if (opcode == (byte)0x31)
            {
                if (opMode == AddressMode.RegVal)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) / Get32BitParameter(IP + 2)));
                }
                else if (opMode == AddressMode.RegReg)
                {
                    SetRegister(ram.memory[IP + 1], (GetRegister(ram.memory[IP + 1]) / GetRegister(ram.memory[IP + 2])));
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction DIV does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #region [Unfinished] INC (Increment, 0x08)
            else if (opcode == (byte)0x08)
            {
                if (opMode == AddressMode.RegVal)
                {
                    int i = GetRegister(ram.memory[IP + 1]);
                    i++;
                    SetRegister(ram.memory[IP + 1], i);
                }
                else if (opMode == AddressMode.RegReg)
                {
                    int i = GetRegister(ram.memory[IP + 1]);
                    i++;
                    SetRegister(ram.memory[IP + 1], i);
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction INC does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #region [Unfinished] DEC (Decrement, 0x09)
            else if (opcode == (byte)0x08)
            {
                if (opMode == AddressMode.RegVal)
                {
                    int i = GetRegister(ram.memory[IP + 1]);
                    i--;
                    SetRegister(ram.memory[IP + 1], i);
                }
                else if (opMode == AddressMode.RegReg)
                {
                    int i = GetRegister(ram.memory[IP + 1]);
                    i--;
                    SetRegister(ram.memory[IP + 1], i);
                }
                else
                {
                    throw new Exception("[CRITICAL ERROR] The instruction INC does not support addressing mode: " + AdMode);
                }
                PC += 5;
            }
            #endregion
            #endregion

            #region Flow Operations
            #region [COMPLETE] CLL (Call, 0x11)
            else if (opcode == (byte)0x11)
            {
                // Parameter 1: Memory Location
                // Add the next position in ram to the call stack
                CallStack.Call((PC + 5));
                // Jump to the location
                parameters[1] = ram.memory[(IP + 1)];
                PC = (byte)parameters[1];
                if (Globals.DebugMode == true)
                {
                    Globals.console.WriteLine("CLL " + parameters[1].ToString());
                }
            }
            #endregion
            #region [COMPLETE] RET (Return, 0x12)
            else if (opcode == (byte)0x12)
            {
                // Get the last location from the call stack and jump to it
                PC = (byte)CallStack.Return();
                if (Globals.DebugMode == true)
                {
                    Globals.console.WriteLine("RET " + PC.ToString());
                }
            }
            #endregion
            #endregion

            #region Stack Control
            #region [Incomplete] PSH (Push, 0x20)
            else if (opcode == 0x20)
            {

            }
            #endregion
            #region [Incomplete] POP (Pop, 0x21)
            else if (opcode == 0x21)
            {

            }
            #endregion
            #endregion

            #region I/O
            #region [Incomplete] INB (Software Interrupt, 0x24)
            else if (opcode == 0x24)
            {

            }
            #endregion
            #region [Incomplete] INW (Software Interrupt, 0x25)
            else if (opcode == 0x25)
            {

            }
            #endregion
            #region [Incomplete] IND (Software Interrupt, 0x26)
            else if (opcode == 0x26)
            {

            }
            #endregion

            #region [Incomplete] OUB (Software Interrupt, 0x27)
            else if (opcode == 0x27)
            {

            }
            #endregion
            #region [Incomplete] OUW (Software Interrupt, 0x28)
            else if (opcode == 0x28)
            {

            }
            #endregion
            #region [Incomplete] OUD (Software Interrupt, 0x29)
            else if (opcode == 0x29)
            {

            }
            #endregion
            #endregion

            #region Interrupts
            #region [Incomplete] SWI (Software Interrupt, 0x2A)
            else if (opcode == (byte)0x2A)
            {

            }
            #endregion
            #region [COMPLETE] KEI (Kernel Interrupt, 0x2B)
            else if (opcode == (byte)0x2B)
            {
                // Parameter 1: Kernel Interrupt to call
                parameters[1] = ram.memory[(IP + 1)];
                StandardLib.KernelInterrupts.HandleInterrupt(parameters[1]);
                PC += 5;
            }

            #endregion
            #endregion

            else
            {
                throw new Exception("[ERROR] The instruction at " + IP + " (" + opcode + ") is not supported by the Apollo IL Runtime");
            }
            NewIP = 0;
            return NewIP;
        }
    }
}
