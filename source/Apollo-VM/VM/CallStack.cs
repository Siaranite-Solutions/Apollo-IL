using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo_IL
{
    /// <summary>
    /// Class containing methods for managing the call stack
    /// </summary>
    public static class CallStack
    {
        /// <summary>
        /// Stack index
        /// </summary>
        private static int stc_index = 0;
        /// <summary>
        /// int array containing the memory locations
        /// </summary>
        private static int[] mem_locations = new int[255];
        /// <summary>
        /// Pushes the location of a bytecode onto the call stack
        /// </summary>
        /// <param name="location"></param>
        public static void Call(int location)
        {
            // Adds the location to the top of the stack (stc_index)
            mem_locations[stc_index] = location;
            // Increments the stack index by 1
            stc_index += 1;
        }
        /// <summary>
        /// Pops and returns the last location in the call stack
        /// </summary>
        /// <returns>The location in the call stack</returns>
        public static int Return()
        {
            // Gets the location from the top of the stack
            int r = mem_locations[stc_index];
            // Overwrites the last location
            mem_locations[stc_index] = 0;
            // Decrements the stack index by 1
            stc_index -= 1;
            // Returns the earlier retrieved location
            return r;    
        }
        
    }
}