using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Apollo_IL.Handlers
{
    public abstract class VConsole
    {
        public abstract void WriteLine(string text);
        public abstract void Write(char ch);
        public abstract void Write(string text);
        public abstract byte Read();
        public abstract string ReadLine();
    }
}