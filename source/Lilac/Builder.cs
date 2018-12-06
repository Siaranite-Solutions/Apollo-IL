using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac.Compiler
{
    class Builder
    {

    }

    class BuildException : Exception
    {
        public int SrcLineNumber = 0;
        public BuildException() : base()
        {

        }
        public BuildException(string Message)
            : base(Message)
        {

        }
        public BuildException(string message, int linenum)
            : base(message)
        {
            this.SrcLineNumber = linenum;
        }
        public BuildException(string message, Exception inner, int linenum)
            : base(message, inner)
        {
            this.SrcLineNumber = linenum;
        }
    }
}
