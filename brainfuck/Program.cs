using System;

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

                    VirtualMachine vm = new VirtualMachine(args[1]);
                    vm.Start();
                    break;
                case "compile":
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Please, provide a source file and a output file directory. Ex.: 314dk compile ./program.314 ./program");
                        return;
                    }
                    Compiler.Compile(args[1], args[2]);
                    break;
                case "debug":
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Please, provide a bytecode file directory as second parameter. Ex.: 314dk debug ./program");
                        return;
                    }
                    break;
            }
        
        }
    }
}
