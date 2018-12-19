using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Decompiler
{
    public class Decompiler
    {
        public Decompiler(byte[] byteCode)
        {
            this.Executable = byteCode;
        }

        /// <summary>
        /// Get the opcode byte of an instruction mnemonic
        /// </summary>
        /// <param name="instr">The instruction mnemonic to get an opcode for</param>
        /// <returns>Instruction opcode byte, returns 0 if not a valid instruction</returns>
        private static string GetInstruction(byte instr)
        {
            //  go through each instruction seeing if the string is equal to that,
            //  if so return the opcode for that instruction, if not return 0
            #region Register Operations
            if (instr == 0x01)
                return "MOV";
            else if (instr == 0x3A)
                return "MOM";
            else if (instr == 0x3B)
                return "MOE";
            else if (instr == 0x02)
                return "SWP";
            else if (instr == 0x1B)
                return "TEQ";
            else if (instr == 0x1C)
                return "TNE";
            else if (instr == 0x1D)
                return "TLT";
            else if (instr == 0x1E)
                return "TGT";
            #endregion

            #region Arithmetic
            else if (instr == 0x04)
                return "ADD";
            else if (instr == 0x05)
                return "SUB";
            else if (instr == 0x08)
                return "INC";
            else if (instr == 0x09)
                return "DEC";
            else if (instr == 0x30)
                return "MUL";
            else if (instr == 0x31)
                return "DIV";
            #endregion
            #region Bitwise Operations
            else if (instr == 0x06)
                return "SHL";
            else if (instr == 0x07)
                return "SHR";
            else if (instr == 0x0F)
                return "ROR";
            else if (instr == 0x0E)
                return "ROL";
            else if (instr == 0x0A)
                return "AND";
            else if (instr == 0x0B)
                return "BOR";
            else if (instr == 0x0C)
                return "XOR";
            else if (instr == 0x0D)
                return "NOT";
            #endregion
            #region Flow Operations
            else if (instr == 0x10)
                return "JMP";
            else if (instr == 0x11)
                return "CLL";
            else if (instr == 0x12)
                return "RET";
            else if (instr == 0x13)
                return "JMT";
            else if (instr == 0x14)
                return "JMF";
            else if (instr == 0x17)
                return "CLT";
            else if (instr == 0x18)
                return "CLF";
            #endregion
            #region Stack Control
            else if (instr == 0x20)
                return "PSH";
            else if (instr == 0x21)
                return "POP";
            #endregion

            #region I/O
            else if (instr == 0x24)
                return "INB";
            else if (instr == 0x25)
                return "INW";
            else if (instr == 0x26)
                return "IND";
            else if (instr == 0x27)
                return "OUB";
            else if (instr == 0x28)
                return "OUW";
            else if (instr == 0x29)
                return "OUD";
            #endregion
            #region Interrupts
            else if (instr == 0x2A)
                return "SWI";
            else if (instr == 0x2B)
                return "KEI";
            #endregion

            else
                return "";

        }


        /// <summary>
        /// Parses a string to see if it matches the VMs registers
        /// </summary>
        /// <param name="Register"></param>
        /// <returns>Register bytecode</returns>
        private static string GetRegister(byte Register)
        {
            // go through each Registerister to see if the provided string is equal to that,
            //  return the correct value if equal. Return 0 if not.
            if (Register == 0xF0)
            {
                return "PC";
            }
            else if (Register == 0xF1)
            {
                return "IP";
            }
            else if (Register == 0xF2)
            {
                return "SP";
            }
            else if (Register == 0xF3)
            {
                return "SS";
            }
            else if (Register == 0xF4)
            {
                return "A";
            }
            else if (Register == 0xF5)
            {
                return "AL";
            }
            else if (Register == 0xF6)
            {
                return "AH";
            }
            else if (Register == 0xF7)
            {
                return "B";
            }
            else if (Register == 0xF8)
            {
                return "BL";
            }
            else if (Register == 0xF9)
            {
                return "BH";
            }
            else if (Register == 0xFA)
            {
                return "C";
            }
            else if (Register == 0xFB)
            {
                return "CL";
            }
            else if (Register == 0xFC)
            {
                return "CH";
            }
            else if (Register == 0xFD)
            {
                return "X";
            }
            else if (Register == 0xFE)
            {
                return "Y";
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// The executable passed to the Decompile() method
        /// </summary>
        private byte[] Executable;
        
        /// <summary>
        /// Each line of code split into array
        /// </summary>
        private string SourceCode;

        /// <summary>
        /// Method compiling the source into the bytecode executable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string Decompile()
        {
            //
            if (Executable.Length == 0)
            {
                throw new Exception("Executable cannot be empty!");
            }
            else
            {

                List<byte[]> splitted = new List<byte[]>();//This list will contain all the splitted arrays.
                int lengthToSplit = 6;

                int arrayLength = Executable.Length;

                for (int i = 0; i < arrayLength; i = i + lengthToSplit)
                {
                    byte[] val = new byte[lengthToSplit];

                    if (arrayLength < i + lengthToSplit)
                    {
                        lengthToSplit = arrayLength - i;
                    }
                    Array.Copy(Executable, i, val, 0, lengthToSplit);
                    splitted.Add(val);


                    foreach (byte[] bytearray in splitted)
                    {
                        byte[] operation = bytearray;
                        string Line = "";
                        byte Instruction = BitOperations.GetFirstSix(operation[0]);
                        byte AddrMode = BitOperations.GetAddressMode(operation[0]);
                        string param1 = "";
                        string param2 = "";

                        string instr = GetInstruction(Instruction);
                        AddressMode AddressMode = (AddressMode)AddrMode;

                        if (AddressMode == AddressMode.RegisterRegister)
                        {
                            param1 = GetRegister(operation[1]);
                            param2 = GetRegister(operation[2]);
                        }
                        else if (AddressMode == AddressMode.ValueRegister)
                        {
                            int num = 0;
                            byte[] value = new byte[] { operation[1], operation[2], operation[3], operation[4] };
                            foreach (byte b in value)
                            {
                                num += b;
                            }
                            param1 = num.ToString();

                            param2 = GetRegister(operation[5]);
                        }
                        else if (AddressMode == AddressMode.RegisterValue)
                        {
                            param1 = GetRegister(operation[1]);
                            byte[] value = new byte[] { operation[2], operation[3], operation[4], operation[5] };
                            int num = 0;
                            foreach (byte b in value)
                            {
                                num += b;
                            }
                            param2 = num.ToString();
                        }
                        else if (AddressMode == AddressMode.ValueValue)
                        {
                            byte[] value1 = new byte[] { operation[2], operation[3] };
                            int num1 = 0;
                            foreach (byte b in value1)
                            {
                                num1 += b;
                            }
                            byte[] value2 = new byte[] { operation[4], operation[5] };
                            int num2 = 0;
                            foreach (byte b in value2)
                            {
                                num2 += b;
                            }
                            param1 = num1.ToString();
                            param2 = num2.ToString();
                        }

                        Line = instr + " " + param1 + " " + param2 + "\n";
                        SourceCode += Line;
                    }


                }
                
                return SourceCode;
            }
            
        }
    }
}
