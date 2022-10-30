using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Program
    {
        private List<Statement> Statements { get; set; }

        public Program()
        {
            Statements = new List<Statement>();
        }

        public Program(List<Statement> statements)
        {
            Statements = statements;
        }
    }
}
