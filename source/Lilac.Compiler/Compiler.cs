using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Compiler
{
    public class Compiler
    {
        public Compiler(string mSourceCode)
        {
            this.SourceCode = mSourceCode;
        }

        /// <summary>
        /// The source code passed to the Compile() method
        /// </summary>
        private string SourceCode;
        /// <summary>
        /// Each line of code split into array
        /// </summary>
        private string[] LinesOfCode;
        /// <summary>
        /// Parts of the current line of code, split via spaces
        /// </summary>
        private string[] Line;
        /// <summary>
        /// Final byte array to be returned through Compile() method
        /// </summary>
        public byte[] ByteCode;
        /// <summary>
        /// Method compiling the source into the bytecode executable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public byte[] Compile()
        {
            //
            if (SourceCode == "")
            {
                throw new BuildException("Source cannot be empty!");
            }
            //
            SourceCode = SourceCode.Replace("\r", "");
            //
            SourceCode = SourceCode.Replace("\' \'", "32");
            LinesOfCode = SourceCode.Split('\n');
            ByteCode = new byte[(LinesOfCode.Length * 6)];
            int CurrentLOC = 0;
            for (int i = 0; i < LinesOfCode.Length; i++)
            {
                // split the code into individual parts
                Line = LinesOfCode[i].Split(' ');
                // check to make sure the instruction isn't too long.
                if (LinesOfCode[i].StartsWith("//"))
                {
                    // Do nothing - the line is a source comment
                }
                else
                {
                    if (Line.Length > 3)
                        throw new BuildException("Instruction too long", (i + 1));
                    // get the instruction code
                    byte instruction;
                    instruction = Instruction.Get(Line[0]);
                    if (instruction == 0)
                        throw new BuildException("Invalid Operation, check spelling", (i + 1));
                    // see what addressing mode is being used and parse the instruction
                    bool reg1 = false;
                    bool reg2 = false;
                    AddressMode AdMode;
                    // in here we also need to store the values for creating the instruction so we need the two parameters if they are given
                    byte para1 = 0;
                    int para2 = 0;

                    if (Line.Length >= 2)
                    {
                        if (CharValue.Check(Line[1]))
                        {
                            reg1 = false;
                            para1 = CharValue.Parse(Line[1]);
                        }
                        else if (VMProcessor.CheckRegister(Line[1]))
                        {
                            Line[1] = Line[1].ToUpper();
                            reg1 = true;
                            para1 = VMProcessor.RetrieveRegister(Line[1]);
                        }
                        else
                        {
                            Line[1] = Line[1].ToUpper();
                            reg1 = false;
                            try
                            {
                                para1 = Byte.Parse(Line[1], System.Globalization.NumberStyles.Integer);
                            }
                            catch (FormatException)
                            {
                                throw new BuildException("Invalid value for parameter 1", (i + 1));
                            }
                            catch (OverflowException)
                            {
                                throw new BuildException("Inevitable overflow for parameter 1", (i + 1));
                            }
                        }
                    }
                    if (Line.Length == 3)
                    {
                        if (CharValue.Check(Line[2]))
                        {
                            reg2 = false;
                            para2 = CharValue.Parse(Line[2]);
                        }
                        else if (VMProcessor.CheckRegister(Line[2]))
                        {
                            Line[2] = Line[2].ToUpper();
                            reg2 = true;
                            para2 = VMProcessor.RetrieveRegister(Line[2]);
                        }
                        else
                        {
                            Line[2] = Line[2].ToUpper();
                            reg2 = false;
                            try
                            {
                                // Parse the second parameter from the third token (Array starts at 0)
                                para2 = Int32.Parse(Line[2], System.Globalization.NumberStyles.Integer);
                            }
                            catch (FormatException)
                            {
                                // Throw a new exception if invalid value for 2nd parameter
                                throw new BuildException("Invalid value for parameter 2", (i + 1));
                            }
                            catch (OverflowException)
                            {
                                // Throw a new exception if probable overflow
                                throw new BuildException("Inevitable overflow for parameter 2", (i + 1));
                            }
                        }
                    }
                    // now to get the addressing mode into the format we need it in
                    if (reg1 == true && reg2 == true)
                        AdMode = AddressMode.RegisterRegister;
                    else if (reg1 == true && reg2 == false)
                        AdMode = AddressMode.RegisterValue;
                    else if (reg1 == false && reg2 == true)
                        AdMode = AddressMode.ValueRegister;
                    else
                        AdMode = AddressMode.ValueValue;
                    // we need a byte instruction

                    byte[] operation = new Instruction(instruction, AdMode, para1, para2).InstructionBytecode;
                    // finally create an instruction and append it to the array
                    for (int j = 0; j < operation.Length; j++)
                    {
                        ByteCode[(j + CurrentLOC)] = operation[j];
                    }
                    // add six to the current location in the exe as we have just appended a new instruction
                    CurrentLOC += 6;
                    // let the whole thing go around again
                }
            }
            return ByteCode;
        }
    }
}
