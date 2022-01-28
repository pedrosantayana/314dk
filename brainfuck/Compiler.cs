using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevelopmentKit
{
    enum Instruction : byte
    {
        IncrementPointer = 0x00,
        DecrementPointer = 0x01,
        Increment = 0x02,
        Decrement = 0x03,
        Print = 0x04,
        Read = 0x05,
        Start = 0x06,
        Stop = 0x07,
        Push = 0x08,
        Pop = 0x09,
        Peek = 0x0A,
        Sum = 0x0B,
        Sub = 0x0C,
        AND = 0x0D,
        OR = 0x0E,
        XOR = 0x0F
    }
    class Compiler
    {
        public static void CompileFile(string inputFileDirectory, string outputFileDirectory)
        {
            string[] sourceCode = File.ReadAllLines(inputFileDirectory);
            FileStream outputFile = File.Create(outputFileDirectory);
            byte[] preCode = Compiler.CompileProgram(sourceCode);
            byte[] bytecode = Compiler.Optimize(preCode);

            Console.WriteLine(bytecode.Length);
            outputFile.Write(bytecode);
            outputFile.Close();
        }

        private static byte[] CompileLine(string line)
        {
            string[] words = line.Split(' ');
            List<byte> output = new List<byte>();
            int arg = 1;

            switch (words[0].Trim())
            {
                case "next":
                    if (words.Length == 2)
                        Int32.TryParse(words[1], out arg);
                    for (int i = 0; i < arg; i++)
                        output.Add((byte)Instruction.IncrementPointer);
                    break;
                case "back":
                    if (words.Length == 2)
                        Int32.TryParse(words[1], out arg);
                    for (int i = 0; i < arg; i++)
                        output.Add((byte)Instruction.DecrementPointer);
                    break;
                case "add":
                    if (words.Length == 2)
                        Int32.TryParse(words[1], out arg);
                    for (int i = 0; i < arg; i++)
                        output.Add((byte)Instruction.Increment);
                    break;
                case "sub":
                    if (words.Length == 2)
                        Int32.TryParse(words[1], out arg);
                    for (int i = 0; i < arg; i++)
                        output.Add((byte)Instruction.Decrement);
                    break;
                case "print":
                    output.Add((byte)(0x04));
                    break;
                case "read":
                    output.Add((byte)(0x05));
                    break;
                case "start":
                    output.Add((byte)(0x06));
                    break;
                case "stop":
                    output.Add((byte)(0x07));
                    break;
                case "push":
                    output.Add((byte)(0x08));
                    break;
                case "pop":
                    output.Add((byte)(0x09));
                    break;
                case "peek":
                    output.Add((byte)(0x0A));
                    break;
                case "adds":
                    output.Add((byte)(0x0B));
                    break;
                case "subs":
                    output.Add((byte)(0x0C));
                    break;
                case "and":
                    output.Add((byte)(0x0D));
                    break;
                case "or":
                    output.Add((byte)(0x0E));
                    break;
                case "xor":
                    output.Add((byte)(0x0F));
                    break;
                case "store": // not working yet
                    if (words.Length == 3 && words[1] == "hex")
                    {
                        string hexString = words[2];
                        byte[] data = StringToByteArray(hexString);
                        foreach (byte dataByte in data)
                        {
                            for (int i = 0; i < dataByte; i++)
                                output.Add((byte)Instruction.Increment);
                            output.Add((byte)Instruction.IncrementPointer);
                        }
                    }
                    break;
            }
            return output.ToArray();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
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

        public static byte[] CompileProgram(string[] sourceCode)
        {
            Dictionary<string, byte[]> funcs = new Dictionary<string, byte[]>();
            List<string> mainSource = new List<string>();
            List<byte> outputBytecode = new List<byte>();

            for (int i = 0; i < sourceCode.Length; i++)
            {
                string line = sourceCode[i];
                if (line.Split().Length == 2 && line.Split()[0] == "fn")
                {
                    string funcName = line.Split()[1];
                    List<string> funcSource = new List<string>();

                    if (funcName == "main")
                    {
                        i++;
                        while (sourceCode[i] != "end")
                            mainSource.Add(sourceCode[i++]);
                        continue;
                    }

                    while (sourceCode[i] != "end")
                        funcSource.Add(sourceCode[++i]);

                    List<byte> bytecode = new List<byte>();
                    foreach (string _line in funcSource)
                    {
                        bytecode.AddRange(Compiler.CompileLine(_line));
                    }

                    funcs.Add(funcName, bytecode.ToArray());
                }
            }

            for (int i = 0; i < mainSource.Count; i++)
            {
                string line = mainSource[i];
                if (line.Split().Length == 2 && line.Split()[0] == "call")
                {
                    string funcName = line.Split()[1];
                    if (!funcs.ContainsKey(funcName))
                    {
                        // Invalid function name
                    }
                    outputBytecode.AddRange(funcs[funcName]);
                    continue;
                }
                outputBytecode.AddRange(Compiler.CompileLine(line));
            }

            return outputBytecode.ToArray();
        }
    }
}
