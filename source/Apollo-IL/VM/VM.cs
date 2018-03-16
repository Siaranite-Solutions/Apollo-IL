using System;

namespace Apollo_IL
{
    public class VM
    {
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
		/// <param name="executable"></param>
		/// <param name="ramsize"></param>
		public VM(byte[] executable, int ramsize)
		{
			ram = new RandomAccessMemory(ramsize);
			//Loads the executable into the Virtual Machine's memory through LoadApplication()
			LoadApplication(executable);
		}
		/// <summary>
		/// Executes the binary loaded into the Virtual Machine's memory
		/// </summary>
		public void Execute()
		{
			while (ram.memory[IP] != 0x00)
			{

				IP = PC;
				PC++;
			}
		}
		/// <summary>
		/// Retrieves a bit from a specified byte
		/// false if 0, true if 1
		/// </summary>
		/// <param name="b"></param>
		/// <param name="bitNumber"></param>
		/// <returns></returns>
		private bool GetBit(byte b, int bitNumber)
        {
            return ((int)b & (1 << bitNumber)) != 0;
        }
    }
}
