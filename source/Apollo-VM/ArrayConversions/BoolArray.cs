using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Apollo_IL.Conversions
{
    /// <summary>
    /// Contains the bool[] methods for joining two boolean arrays
    /// </summary>
    public static class BooleanArray
    {
        /// <summary>
        /// Adds two boolean arrays (bool_1, bool_2) together and returns the new array
        /// </summary>
        /// <param name="bool_1"></param>
        /// <param name="bool_2"></param>
        /// <returns>Array two joined at the end of Array one</returns>
        public static bool[] JoinBooleans(bool[] bool_1, bool[] bool_2)
        {
            bool[] ret = new bool[(bool_1.Length + bool_2.Length)];
            for (int i = 0; i < bool_1.Length; i++)
            {
                ret[i] = bool_1[i];
            }
            for (int i = 0; i < bool_2.Length; i++)
            {
                ret[(i + bool_1.Length)] = bool_2[i];
            }
            return ret;
        }
    }
}