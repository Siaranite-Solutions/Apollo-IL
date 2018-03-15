using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo_IL
{
    public class RandomAccessMemory
    {
		/// <summary>
		/// Memory as a byte array
		/// </summary>
		public byte[] memory;
		/// <summary>
		/// Constructor for the RAM
		/// </summary>
		/// <param name="amount"></param>
		public RandomAccessMemory(int amount)
		{
			memory = new byte[amount];
		}
		/// <summary>
		/// Position of where the executed binary ends + 1
		/// </summary>
		public int RAMLimit;

    }
}
