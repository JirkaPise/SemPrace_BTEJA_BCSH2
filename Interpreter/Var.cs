using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public class Var
    {
        public string Name { get; set; }
        public object? Value { get; set; }
        public TokenType Type { get; set; }

        public Var(string name, object? value, TokenType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
