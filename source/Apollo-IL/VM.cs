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
		/// 
		/// </summary>
		private AddressMode opMode;

		#region registers
		/// <summary>
		/// Program Counter
		/// </summary>
		public byte PC;
		/// <summary>
		/// Stack Pointer
		///
		public byte SP;
		/// <summary>
		/// Instruction Pointer
		/// </summary>
		public byte IP;
		/// <summary>
		/// 
		/// </summary>
		public byte SS;
		/// <summary>
		/// 
		/// </summary>
		public byte AL;
		/// <summary>
		/// 
		/// </summary>
		public byte AH;
		/// <summary>
		/// 
		/// </summary>
		public byte BL;
		/// <summary>
		/// 
		/// </summary>
		public byte BH;
		/// <summary>
		/// 
		/// </summary>
		public byte CL;
		/// <summary>
		/// 
		/// </summary>
		public byte CH;
		/// <summary>
		/// 
		/// </summary>
		public byte DL;
		/// <summary>
		/// 
		/// </summary>
		public byte DH;
		/// <summary>
		/// 
		/// </summary>
		public Int32 Y;
		/// <summary>
		/// 
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
    }
}
