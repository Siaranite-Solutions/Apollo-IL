using System;
using Apollo_IL.StandardLib;
using Apollo_IL.Conversions;

namespace Apollo_IL
{
    public partial class VM
    {
		private bool LastLogic;
		/// <summary>
		/// Contains the byte value of the new Instruction Pointer
		/// </summary>
		private byte NewIP;
		/// <summary>
		/// Current instructions parameters
		/// </summary>
		private int[] parameters;
		/// <summary>
		/// enum of the Address Modes
		/// </summary>
		private enum AddressMode
		{
			RegReg,
			RegVal,
			ValVal,
			ValReg
		}

		/// <summary>
		/// Address mode for current operation
		/// </summary>
		private AddressMode opMode;

		#region registers
		/// <summary>
		/// Program Counter
		/// </summary>
		public byte PC;
		/// <summary>
		/// Stack Pointer
		/// </summary>
		public byte SP;
		/// <summary>
		/// Instruction Pointer
		/// </summary>
		public byte IP;
		/// <summary>
		/// Stack segment
		/// </summary>
		public byte SS;
		/// <summary>
		/// General purpose register
		/// Lower byte of the A register
		/// </summary>
		public byte AL;
		/// <summary>
		/// General purpose register
		/// Higher byte of the A register
		/// </summary>
		public byte AH;
		/// <summary>
		/// General purpose register
		/// Lower byte of the B register
		/// </summary>
		public byte BL;
		/// <summary>
		/// General purpose register
		/// Higher byte of the B register
		/// </summary>
		public byte BH;
		/// <summary>
		/// General purpose register
		/// Lower byte of the C regiser
		/// </summary>
		public byte CL;
		/// <summary>
		/// General purpose register
		/// Higher byte of the C register
		/// </summary>
		public byte CH;
		/// <summary>
		/// 32-bit general purpose register
		/// </summary>
		public Int32 Y;
		/// <summary>
		/// 32-bit general purpose register
		/// </summary>
		public Int32 X;
		#endregion
		

		#region BitOperations
		


		#endregion


		/// <summary>
		/// Loads the application as a byte array into the virtual machine's memory
		/// </summary>
		/// <param name="application"></param>
		private void LoadApplication(byte[] application)
		{
			int i = 0;
			while (i < application.Length)
			{
				ram.memory[i] = application[i];
				i++;
			}
			//Sets the RAM limit, right above the end of the executable:
			ram.RAMLimit = (i + 1);

		}

		public RandomAccessMemory ram;
		/// <summary>
		/// Constructor for a new instance of a virtual machine, specifiying the executable to run and the VM's amount of RAM.
		/// </summary>
		/// <param name="executable">The executing binary's size must be greater than the ramsize</param>
		/// <param name="ramsize">The size of virtual memory in bytes must be larger than the size of the executable binary</param>
		public VM(byte[] executable, int ramsize)
		{
			ram = new RandomAccessMemory(ramsize);
			// Declares the six-bit instruction to a new boolean array of length 9 
			sixbits = new bool[9];
			// Declares the two-bit parameter addressing mode to a new boolean array of length 2
			twobits = new bool[2];
			// Defines the parameters integer array as an array of length 5
			parameters = new int[5];
			// Sets the instruction pointer to 0
			IP = 0;
			// Sets the program counter to 1
			PC = 1;
			// Sets the parent virtual machine for the standard library to this instance
			KernelInterrupts.ParentVM = this;
			//Loads the executable into the Virtual Machine's memory through LoadApplication()
			LoadApplication(executable);
		}
		/// <summary>
		/// Address mode initially set to 0, for Register:Register
		/// </summary>
		private int AdMode = 0;

		private int GetRegister(byte Reg)
		{
			if (Reg == (byte)0xF0)
				return (int)PC;
			else if (Reg == (byte)0xF1)
				return (int)IP;
			else if (Reg == (byte)0xF2)
				return (int)SP;
			else if (Reg == (byte)0xF3)
				return (int)SS;
			else if (Reg == (byte)0xF4)
				return GetSplit('A');
			else if (Reg == (byte)0xF5)
				return AL;
			else if (Reg == (byte)0xF6)
				return AH;
			else if (Reg == (byte)0xF7)
				return GetSplit('B');
			else if (Reg == (byte)0xF8)
				return BL;
			else if (Reg == (byte)0xF9)
				return BH;
			else if (Reg == (byte)0xFA)
				return GetSplit('C');
			else if (Reg == (byte)0xFB)
				return CL;
			else if (Reg == (byte)0xFC)
				return CH;
			else if (Reg == (byte)0xFD)
				return X;
			else if (Reg == (byte)0xFE)
				return Y;
			else
				return 0;
		}
		/// <summary>
		/// Gets the current address mode from the specified byte
		/// </summary>
		/// <param name="b">Address mode byte</param>
		private void GetAddressMode(byte b)
		{
			AdMode = GetLastTwo(b);
			if (AdMode == 0)
			{
				opMode = AddressMode.RegReg;
			}
			else if (AdMode == 1)
			{
				opMode = AddressMode.RegVal;
			}
			else if (AdMode == 2)
			{
				opMode = AddressMode.ValVal;
			}
			else if (AdMode == 3)
			{
				opMode = AddressMode.ValReg;
			}
			else
			{
				throw new Exception("<Critical Error!> Address mode at " + IP + " (" + AdMode + ") is invalid.");
			}
		}
		/// <summary>
		/// Executes the binary loaded into the Virtual Machine's memory
		/// </summary>
		public void Execute()
		{
			while (ram.memory[IP] != 0x00)
			{
				/// <summary>
				/// Gets the operation from the first six bytes of the instruction pointer
				/// </summary>
				/// <returns>operation from instruction pointer</returns>
				byte opcode = (byte)GetFirstSix(ram.memory[IP]);
				GetAddressMode(ram.memory[IP]);
				//ParseOpCode(opcode);
				IP = PC;
				PC++;
			}
		}
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
                sixbits[i] = BitOps.GetBit(b, i);
            }
            return BitOps.GetIntegerValue(sixbits);
        }
		private bool[] sixbits;
        private int GetFirstSix(byte b)
        {
            for (int i = 0; i < 6; i++)
            {
                sixbits[i] = BitOps.GetBit(b, i);
            }
            return BitOps.GetIntegerValue(sixbits);
        }
        private bool[] twobits;

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
                return BitOps.CombineBytes(AL, AH);
            }
            else if (register == 'B')
            {
                return BitOps.CombineBytes(BL, BH);
            }
            else if (register == 'C')
            {
                return BitOps.CombineBytes(CL, CH);
            }
            throw new Exception("There was an internal error and the VM has closed to protect your data. Please report this to the application developer");
        }
    }
}
