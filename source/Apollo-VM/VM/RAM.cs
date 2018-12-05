using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo_IL
{
    public class RandomAccessMemory
    {
		/// <summary>
		/// Fills the specified location in memory with the specified byte content
		/// </summary>
		/// <param name="location"></param>
		/// <param name="content"></param>
		public void SetSection(int location, byte[] content)
		{
            for (int i = 0; i < content.Length; i++)
            {
                memory[(i + location)] = content[i];
            }
        }

		/// <summary>
		/// Returns a section of memory at the specified location of the specified length
		/// </summary>
		/// <param name="location"></param>
		/// <param name="length"></param>
		/// <returns>Section of memory</returns>
		public byte[] GetSection(int location, int length)
		{
            byte[] ret = new byte[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = memory[(location + i)];
            }
            return ret;
        }
		/// <summary>
		/// Sets a specified location in memory to a specified byte
		/// </summary>
		/// <param name="location"></param>
		/// <param name="content"></param>
		public void SetByte(int location, byte content)
		{
            if (location > RAMLimit)
            {
                memory[location] = content;
            }
            else
            {
                throw new Exception("The application tried to modify itself and was terminated. Consult the application vendor for more support.");
            }
        }
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
