using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lilac.IDE
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Apollo IL Integrated Development Environment";
            if (args.Length > 0)
            {
                if (args[0].Length <= 6 && args[0].EndsWith(".ila"))
                {
                    
                    Debugger miniMode = new Debugger(File.ReadAllBytes(args[0]), true);
                    miniMode.Run();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (args[0].Length <= 5 && args[0].EndsWith(".lsf"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    Console.WriteLine("Loading editor and debugger...");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainWindow(args[0]));

                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                Console.WriteLine("Loading editor and debugger...");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
