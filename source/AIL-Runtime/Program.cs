using System;
using Apollo_IL;
using System.IO;

namespace AIL_Runtime
{
    class Program
    {
        public static byte[] HelloWorld =
        {
            65, 245, 2, 0, 0, 0, 
65, 253, 250, 0, 0, 0, 
250, 69, 250, 0, 0, 0, 
250, 110, 251, 0, 0, 0, 
250, 116, 252, 0, 0, 0, 
250, 101, 253, 0, 0, 0, 
250, 114, 254, 0, 0, 0, 
250, 32, 255, 0, 0, 0, 
250, 89, 0, 1, 0, 0, 
250, 111, 1, 1, 0, 0, 
250, 117, 2, 1, 0, 0, 
250, 114, 3, 1, 0, 0, 
250, 32, 4, 1, 0, 0, 
250, 78, 5, 1, 0, 0, 
250, 97, 6, 1, 0, 0, 
250, 109, 7, 1, 0, 0, 
250, 101, 8, 1, 0, 0, 
250, 58, 9, 1, 0, 0, 
250, 32, 10, 1, 0, 0, 
65, 247, 17, 0, 0, 0, 
235, 1, 0, 0, 0, 0, 
65, 245, 4, 0, 0, 0, 
65, 253, 244, 1, 0, 0, 
235, 1, 0, 0, 0, 0, 
250, 72, 237, 1, 0, 0, 
250, 101, 238, 1, 0, 0, 
250, 108, 239, 1, 0, 0, 
250, 108, 240, 1, 0, 0, 
250, 111, 241, 1, 0, 0, 
250, 44, 242, 1, 0, 0, 
250, 32, 243, 1, 0, 0, 
65, 253, 237, 1, 0, 0, 
68, 247, 7, 0, 0, 0, 
65, 245, 2, 0, 0, 0, 
235, 1, 0, 0, 0, 0, 
65, 245, 1, 0, 0, 0, 
65, 246, 10, 0, 0, 0, 
235, 1, 0, 0, 0, 0, 
208, 0, 0, 0, 0, 0, 
        };
        static void Main(string[] args)
        {
            byte[] LoadedApplication;
            Apollo_IL.Globals.console = new AIL_Runtime.AR_Console();
            if (args.Length == 0)
            {
                Console.Title = "Apollo-VM Runtime - Hello World!";
                LoadedApplication = HelloWorld;
                try
                {
                    Apollo_IL.Executable.Run(LoadedApplication);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + "\nPress any key to terminate...");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
