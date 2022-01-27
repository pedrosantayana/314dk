using System;
using System.IO;

namespace DevelopmentKit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("314VM v0.0.1"); 
            if (args.Length == 0)
            {
                Console.WriteLine("Commands avaliable:");
                Console.WriteLine("run <bytecode file>: Runs a bytecode file.");
                Console.WriteLine("compile <source file> <output file>: Compile a source file and save the bytecode on output file.");
                return;
            }

            switch (args[0])
            {
                case "run":
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Please, provide a bytecode file directory as second parameter. Ex.: 314dk run ./program");
                        return;
                    }
                    byte[] bytecode = File.ReadAllBytes(args[1]);
                    VirtualMachine vm = new VirtualMachine(Compiler.BytecodeToInstructions(bytecode));
                    vm.Start();
                    break;
                case "compile":
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Please, provide a source file and a output file directory. Ex.: 314dk compile ./program.314 ./program");
                        return;
                    }
                    Compiler.CompileFile(args[1], args[2]);
                    break;
                case "debug":
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Please, provide a bytecode file directory as second parameter. Ex.: 314dk debug ./program.314");
                        return;
                    }
                    string[] sourceCode = File.ReadAllLines(args[1]);

                    Console.Write("Compiling... ");
                    byte[] instructions = Compiler.Compile(sourceCode);
                    Console.WriteLine("Compiled {0} instructions.", instructions.Length);

                    VirtualMachine vma = new VirtualMachine(Compiler.Compile(sourceCode));
                    Console.WriteLine("[Program execution]");
                    vma.Start();
                    Console.WriteLine("\n[Memory dump]");
                    for (int i=0; i < 5; i++)
                    {
                        for (int j=0; j < 20; j++)
                        {
                            Console.Write("{0} ", vma.memory[i * 20 + j]);
                        }
                        Console.WriteLine("");
                    }
                    break;
            }
        
        }
    }
}
