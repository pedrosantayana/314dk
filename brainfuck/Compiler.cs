using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DevelopmentKit
{
    enum Token
    {
        Instruction
    }

    class Instruction
    {
        string[] args;
        
        public Instruction(string sourceLine)
        {

        }
    }
    class Compiler
    {
        public static void CompileFile(string inputFileDirectory, string outputFileDirectory)
        {
            string[] sourceCode = File.ReadAllLines(inputFileDirectory);
            FileStream outputFile = File.Create(outputFileDirectory);
            byte[] preCode = Compiler.Compile(sourceCode);
            byte[] bytecode = Compiler.Optimize(preCode);

            Console.WriteLine(bytecode.Length);
            outputFile.Write(bytecode);
            outputFile.Close();
        }
        public static byte[] Compile(string[] sourceCode)
        {
            List<byte> output = new List<byte>();
            foreach (string line in sourceCode)
            {
                string[] lineSplit = line.Split(' ');

                if (lineSplit.Length == 0)
                    continue;

                switch (lineSplit[0])
                {
                    case "nop":
                        output.Add((byte)(0x00));
                        break;
                    case "next":
                        byte nextValue = Convert.ToByte(lineSplit[1]);
                        for (int i = 0; i < nextValue; i++)
                            output.Add((byte)(0x01));
                        break;
                    case "back":
                        byte backValue = Convert.ToByte(lineSplit[1]);
                        for (int i = 0; i < backValue; i++)
                            output.Add((byte)(0x02));
                        break;
                    case "add":
                        byte addValue = Convert.ToByte(lineSplit[1]);
                        for (int i = 0; i < addValue; i++)
                            output.Add((byte)(0x03));
                        break;
                    case "sub":
                        byte subValue = Convert.ToByte(lineSplit[1]);
                        for (int i = 0; i < subValue; i++)
                            output.Add((byte)(0x04));
                        break;
                    case "print":
                        output.Add((byte)(0x05));
                        break;
                    case "read":
                        output.Add((byte)(0x06));
                        break;
                    case "start":
                        output.Add((byte)(0x07));
                        break;
                    case "stop":
                        output.Add((byte)(0x08));
                        break;
                    case "push":
                        output.Add((byte)(0x09));
                        break;
                    case "pop":
                        output.Add((byte)(0x0A));
                        break;
                    case "peek":
                        output.Add((byte)(0x0B));
                        break;
                        
                }
            }
            return output.ToArray();
        }
        private static byte[] Optimize(byte[] preCode)
        {
            if (preCode.Length % 2 != 0)
            {
                Array.Resize(ref preCode, preCode.Length + 1);
            }

            byte[] output = new byte[preCode.Length / 2];

            for (int i = 0; i < preCode.Length; i += 2)
            {
                byte code = (byte)((preCode[i] << 4) | (preCode[i + 1]));
                output[i / 2] = code;
            }
            return output;
        }

        public static byte[] BytecodeToInstructions(byte[] bytecode)
        {
            byte[] instructions = new byte[bytecode.Length * 2];
            for (int i = 0; i < bytecode.Length; i++)
            {
                instructions[i * 2] = (byte)(bytecode[i] >> 4);
                instructions[i * 2 + 1] = (byte)(bytecode[i] & 0b00001111);
            }
            return instructions;
        }



        private static byte[] FunctionGenerator(string[] sourceCode)
        {
            Hashtable funcs = new Hashtable();
            for (int i = 0; i < sourceCode.Length; i++)
            {
                string line = sourceCode[i];
                if (line.Split().Length == 2 && line.Split()[0] == "fn")
                {
                    string funcName = line.Split()[1];
                    List<string> funcSource = new List<string>();
                    i++;
                    while (sourceCode[i] != "end")
                    {
                        funcSource.Add(sourceCode[i]);
                        i++;
                    }
                    funcs.Add(funcName, Compiler.Compile(funcSource.ToArray()));

                }
            }

            string[] mainFunc;
        }
    }
}
