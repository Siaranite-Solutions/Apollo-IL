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
            // stdio Commands
            #region stdio
            if (command == 0x01)
            {
                if (Globals.DebugMode == true)
                {
                    Globals.console.Write("KEI 0x01");
                }
                if (ParentVM.AL == 0x01)
                {
                    Globals.console.Write((char)ParentVM.AH);
                    if (Globals.DebugMode == true)
                        Globals.console.WriteLine(" 0x01");
                }
                else if (ParentVM.AL == 0x02)
                {
                    string toPrint = "";
                    byte[] toConvert = new byte[ParentVM.GetSplit('B')];
                    toConvert = ParentVM.ram.GetSection(ParentVM.X, ParentVM.GetSplit('B'));
                    for (int i = 0; i < toConvert.Length; i++)
                    {
                        toPrint += (char)toConvert[i];
                    }
                    Globals.console.Write(toPrint);
                    if (Globals.DebugMode == true)
                        Globals.console.WriteLine(" 0x02");
                }
                else if (ParentVM.AL == 0x03)
                {
                    ParentVM.AH = (byte)Globals.console.Read();
                }
                else if (ParentVM.AL == 0x04)
                {
                    string toConvert = Globals.console.ReadLine();
                    byte[] toWrite = new byte[toConvert.Length];
                    for (int i = 0; i < toWrite.Length; i++)
                    {
                        toWrite[i] = (byte)toConvert[i];
                    }
                    ParentVM.SetSplit('B', toWrite.Length);
                    ParentVM.ram.SetSection(ParentVM.X, toWrite);
                }
            }
            else if (command == 0x02)
            {
                ParentVM.Halt();
            }
            else
            {
                Globals.console.WriteLine("Undocumented function" + command + "\nHalting for protection of data");
                ParentVM.Halt();
            }
            #endregion
        }
    }
}