using System;
using System.Collections.Generic;
using System.IO;

namespace DevelopmentKit
{
    class Compiler
    {
        public static void Compile(string inputFileDirectory, string outputFileDirectory)
        {
            // Console.WriteLine("Compiling {}.", inputFileDirectory);
            string[] sourceCode = File.ReadAllLines(inputFileDirectory);
            FileStream outputFile = File.Create(outputFileDirectory);
            List<byte> output = new List<byte>();
            foreach (string line in sourceCode)
            {
                string[] lineSplit = line.Split(' ');
                switch (lineSplit[0])
                {
                    case "next":
                        output.Add((byte)(0x01));
                        break;
                    case "back":
                        output.Add((byte)(0x02));
                        break;
                    case "add":
                        byte addValue = Convert.ToByte(lineSplit[1]);
                        for (int i=0; i < addValue; i++)
                        {
                            output.Add((byte)(0x03));
                        }
                        break;
                    case "sub":
                        byte subValue = Convert.ToByte(lineSplit[1]);
                        for (int i = 0; i < subValue; i++)
                        {
                            output.Add((byte)(0x04));
                        }
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
                    case "end":
                        output.Add((byte)(0x08));
                        break;
                }
            }

            if (output.Count % 2 != 0)
            {
                output.Add((byte)(0x00));
            }

            byte[] preCode = output.ToArray();
            output.Clear();

            for (int i=0; i < preCode.Length; i+=2)
            {
                byte code = (byte)((preCode[i] << 4) | (preCode[i + 1]));
                output.Add(code);
            }
            Console.WriteLine(output.Count);
            outputFile.Write(output.ToArray());
            outputFile.Close();
        }
    }
}
