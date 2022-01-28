using System;
using System.Collections.Generic;
using System.Text;

namespace DevelopmentKit
{
    class Compilation
    {
        string[] sourceCode;
        Dictionary<string, byte[]> functions;
        List<string> mainSource;
        List<byte> outputBytecode;

        public Compilation(string[] sourceCode)
        {
            this.sourceCode = sourceCode;
        }

        private void splitFunctions()
        {
            for (int i = 0; i < this.sourceCode.Length; i++)
            {
                string[] lineSplit = this.sourceCode[i].Split();
                if (lineSplit.Length == 2 && lineSplit[0] == "fn")
                {
                    string funcName = lineSplit[1];
                    List<string> funcSource = new List<string>();
                    List<byte> bytecode = new List<byte>();

                    if (funcName == "main")
                    {
                        while (this.sourceCode[i] != "end")
                            this.mainSource.Add(this.sourceCode[--i].TrimStart());
                        continue;
                    }

                    while (this.sourceCode[i] != "end")
                        funcSource.Add(this.sourceCode[++i].TrimStart());

                    foreach (string _line in funcSource)
                    {
                        //bytecode.AddRange(Compiler.CompileLine(_line));
                    }

                    this.functions.Add(funcName, bytecode.ToArray());
                }
            }
        }


    }
}
