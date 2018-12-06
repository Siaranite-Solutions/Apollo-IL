using System;
using System.IO;

namespace Lilac.Compiler
{
    class Program
    {
        /// <summary>
        /// Path to the source to be compiled
        /// </summary>
        private static string FilePath = "";

        private static FileInfo FI;

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        
        static void Main(string[] args)
        {
            // While loop checks for a valid filename
            bool ValidFilename = false;
            while (ValidFilename == false)
            {
                if (args.Length == 1)
                {
                    if (args[0].Length <= 6 && args[0].EndsWith(".lsf"))
                    {
                        FilePath = args[0];
                        FI = null;
                        try
                        {
                            FI = new FileInfo(FilePath);
                            // If a valid filename for source was passed to the compiler via command line arguments
                            ValidFilename = true;
                        }
                        catch (Exception)
                        {
                            // If an invalid filename was passed, ask for another...
                            Console.WriteLine("Please enter a valid filename for source to be compiled:");
                            FilePath = Console.ReadLine();
                            // Go back to beginning of loop...
                        }
                    }
                }
                // If no command line arguments were passed...
                else
                {
                    FileInfo FI = null;
                    try
                    {
                        if (FilePath != "" && (FilePath.Length <= 6 && FilePath.EndsWith(".lsf")))
                        {
                            FI = new FileInfo(FilePath);
                            ValidFilename = true;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Please enter a valid filename for source to be compiled:");
                        FilePath = Console.ReadLine();
                    }


                    ValidFilename = true;
                }
            }

            byte[] VMExecutable = null;
            string SourceCode = File.ReadAllText(FilePath);
            Compiler CompiledSource = new Compiler(SourceCode);
            try
            {
                Console.WriteLine("Compiling...");
                VMExecutable = CompiledSource.Compile();
                Console.WriteLine("Please enter where you'd like to save the compiled executable: (Must end in .ila - will add automatically if not added)");
                string path = Console.ReadLine();
                // If the path doesn't end in a .ila extension, add it
                if (!path.EndsWith(".ila"))
                {
                    path += ".ila";
                }
                File.WriteAllBytes(path, VMExecutable);
            }
            catch (BuildException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR]" + ex.Message);
                Console.WriteLine("Error occured at: " + ex.SrcLineNumber);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR]" + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
