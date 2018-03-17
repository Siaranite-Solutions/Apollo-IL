using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo_IL.StandardLib
{
    public static class KernelInterrupts
    {
        public static VM ParentVM;
        public static void HandleInterrupt(int command)
        {
            #region standard I/O commands
            if (command == 0x01)
            {
                if (ParentVM.AL == 0x01)
                {

                }
            }
            #endregion
        }
    }
}