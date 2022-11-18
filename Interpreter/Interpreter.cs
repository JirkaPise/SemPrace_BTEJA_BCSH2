using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public class Interpreter
    {
        private Lexer lexer;
        private SemPrace_BTEJA_BCSH2.Parser.Parser parser;

        private string path;
        public string ConsoleOutput => ProgramContext.ConsoleOutput;
        private ProgramContext ProgramContext { get; set; }

        public Interpreter()
        {
            ProgramContext = new();
        }

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                using (StreamReader reader = new StreamReader(path))
                {
                    Code = reader.ReadToEnd();
                }
            }
        }


        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public bool CodeIsRunning { get; set; }

        public void Interpret()
        {
            CodeIsRunning = true;
            lexer = new Lexer();
            parser = new SemPrace_BTEJA_BCSH2.Parser.Parser();
            List<Token> tokens = lexer.ScanTokens(code);

            var p = parser.Parse(tokens);
            ProgramContext = new ProgramContext();
            ExecutionCntxt c = new ExecutionCntxt(ProgramContext, null);
            p.Evaluate(c);
            ProgramContext.ConsoleOutput += "\n*Program finished*\n";
            CodeIsRunning = false;
        }


    }
}
