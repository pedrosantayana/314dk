using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevelopmentKit
{
    class VirtualMachine
    {
        byte[] program;
        uint instructionPointer = 0;
        byte[] memory = new byte[uint.MaxValue];
        uint pointer = 0;
        Stack<uint> loopStack = new Stack<uint>();

        public VirtualMachine(string bytecodeFileDirectory)
        {
            byte[] programBytecode = File.ReadAllBytes(bytecodeFileDirectory);
            program = new byte[programBytecode.Length * 2];
            for (int i = 0; i < programBytecode.Length; i++)
            {
                program[i * 2] = (byte)(programBytecode[i] >> 4);
                program[i * 2 + 1] = (byte)(programBytecode[i] & 0b00001111);
            }
        }

        public void Start()
        {
            Encoding ascii = Encoding.ASCII;
            while (instructionPointer < program.Length)
            {
                switch (program[instructionPointer])
                {
                    case 0x00:
                        //nop
                        break;
                    case 0x01:
                        pointer++;
                        break;
                    case 0x02:
                        pointer--;
                        break;
                    case 0x03:
                        memory[pointer]++;
                        break;
                    case 0x04:
                        memory[pointer]--;
                        break;
                    case 0x05:
                        Console.Write(ascii.GetString(new byte[] { memory[pointer] }));
                        break;
                    case 0x06:
                        char inputChar = Console.ReadKey().KeyChar;
                        memory[pointer] = Convert.ToByte(inputChar);
                        break;
                    case 0x07:
                        if (loopStack.Count == 0 || instructionPointer != loopStack.Peek())
                        {
                            loopStack.Push(instructionPointer);
                        }
                        break;
                    case 0x08:
                        if (memory[pointer] != 0)
                        {
                            instructionPointer = loopStack.Peek();
                        }
                        break;
                }
                instructionPointer++;
            }
        }
    }
}
