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
                #if DEBUG
                Globals.console.Write("KEI 0x01");
                #endif
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
                    forconversion = ParentVM.ram.GetSection(ParentVM.X, ParentVM.GetSplit('B'));
                    for (int a = 0; a < forconversion.Length; a++)
                    {
                        ToPrint += (char)forconversion[a];
                    }
                    Globals.console.Write(ToPrint);
                    #if DEBUG
                    Globals.console.WriteLine(" 0x02");
                    #endif
                }
                else if (ParentVM.AL == 0x03)
                {
                    ParentVM.AH = (byte)Globals.console.Read();
                }
                else if (ParentVM.AL == 0x04)
                {
                    string ForConversion = Globals.console.ReadLine();
                    byte[] ToWrite = new byte[ForConversion.Length];
                    for (int a = 0; a < ForConversion.Length; a++)
                    {
                        ToWrite[a] = (byte)ForConversion[a];
                    }
                    ParentVM.SetSplit('B', ToWrite.Length);
                    ParentVM.ram.SetSection(ParentVM.X, ToWrite);
                }
            }
            #endregion
        }
    }
}