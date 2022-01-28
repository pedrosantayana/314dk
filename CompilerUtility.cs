using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DevelopmentKit {
    enum Type {
        Byte = "byte",
        Addr = "addr",
        Hex = "hex",
        String = "str"
    }
    class Function {
        List<Argument> args;
        private string header;
        public string name;
        public List<string> sourceCode;
        public Function(string header, List<string> sourceCode)
        {
            this.header = header;
            this.sourceCode = sourceCode;
        }

        private void getArgs()
        {
            
        }
    }
    class Argument
    {
        Type type { get; set; }
        string name { get; set; }

        public Argument()
        {

        }
    }
    class 
}