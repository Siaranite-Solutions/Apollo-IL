using System;
using System.IO;
using AILCompiler = Lilac.Compiler.Compiler;
using AILDecompiler = Lilac.Decompiler.Decompiler;

namespace Lilac.CLI
{
    class Program
    {
        private static bool UseBuiltinExe = false;

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
            if (UseBuiltinExe == true)
            {
                AILDecompiler Decompiler = new AILDecompiler(HelloWorld);
                string SourceCode = Decompiler.Decompile();
                Console.WriteLine(SourceCode);
                Console.ReadKey(true);
            }
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: -d to decompile a .ila executable\n<source file> to compile a .lsf source file");
            }
            else if (args[0] == "-d")
            {
                if (args[0] != "")
                {
                    AILDecompiler Decompiler = new AILDecompiler(File.ReadAllBytes(args[1]));
                    string SourceCode = Decompiler.Decompile();
                    File.WriteAllText(args[1] + ".lsf", SourceCode);
                }
            }
            else
            {
                #region Compile
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
                AILCompiler CompiledSource = new AILCompiler(SourceCode);
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
                catch (Compiler.BuildException ex)
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
                #endregion
            }
        }
    }
}
