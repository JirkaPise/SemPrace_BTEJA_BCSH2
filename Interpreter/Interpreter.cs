using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public delegate void outputDelegate(string text);
    public delegate string? inputDelegate();

    public class Interpreter
    {
        private Lexer lexer;
        private SemPrace_BTEJA_BCSH2.Parser.Parser parser;

        private string path;

        public outputDelegate Output { get; set; }
        public inputDelegate Input { get; set; }


        public ProgramContext ProgramContext { get; set; }

        public Interpreter()
        {
            ProgramContext = new();
            Output = new outputDelegate(Console.WriteLine);
            Input = new inputDelegate(Console.ReadLine);
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

        public void Interpret()
        {
            lexer = new Lexer();
            parser = new SemPrace_BTEJA_BCSH2.Parser.Parser();
            List<Token> tokens = lexer.ScanTokens(code);

            var p = parser.Parse(tokens);
            ProgramContext = new ProgramContext();
            ProgramContext.Output = Output;
            ProgramContext.Input = Input;
            Output("*Program is running*\n");
            ExecutionCntxt c = new ExecutionCntxt(ProgramContext, null);
            p.Evaluate(c);
            Output("\n*Program finished*");
        }


    }
}
