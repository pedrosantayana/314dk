using System;
using System.Collections.Generic;
using System.Text;

namespace DevelopmentKit
{
    class VirtualMachine
    {
        byte[] instructions;
        int instructionPointer = 0;
        public byte[] memory = new byte[4000000];
        Stack<byte> stack = new Stack<byte>();
        int pointer = 0;
        Stack<int> loopStack = new Stack<int>();

        public VirtualMachine(byte[] instructions)
        {
            this.instructions = instructions;
        }

        public void Start()
        {
            while (instructionPointer < instructions.Length)
            {
                switch (instructions[instructionPointer])
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
                        Console.Write(Encoding.ASCII.GetString(new byte[] { memory[pointer] }));
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
                    case 0x09:
                        stack.Push(memory[pointer]);
                        break;
                    case 0x0A:
                        memory[pointer] = stack.Pop();
                        break;
                    case 0x0B:
                        memory[pointer] = stack.Peek();
                        break;
                    case 0x0C: 
                        break;
                    case 0x0D:
                        break;
                    case 0x0E:
                        break;
                    case 0x0F:
                        break;
                }
                instructionPointer++;
            }
        }
    }
}
