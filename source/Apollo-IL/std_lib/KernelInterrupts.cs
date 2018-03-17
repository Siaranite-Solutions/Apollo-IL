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
                    Globals.console.Write((char)ParentVM.AH);
                    #if DEBUG
                    Globals.console.WriteLine(" 0x01");
                    #endif
                }
                else if (ParentVM.AL == 0x02)
                {
                    string ToPrint = "";
                    byte[] forconversion = new byte[ParentVM.GetSplit('B')];
                    toConvert = ParentVM.ram.GetSection(ParentVM.X, ParentVM.GetSplit('B'));
                }
            }
            #endregion
        }
    }
}