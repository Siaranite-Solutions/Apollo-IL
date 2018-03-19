using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Apollo_IL.Handlers
{
    public class NullConsole : VConsole
    {
        public override byte Read()
        {
            throw new NotImplementedException();
        }
        public override string ReadLine()
        {
            throw new NotImplementedException();
        }
        public override void WriteLine(string text)
        {
            // Do nothing
        }
        public override void Write(char ch)
        {
            // Do nothing
        }
        public override void Write(string text)
        {
            // Do nothing
        }
    }
}