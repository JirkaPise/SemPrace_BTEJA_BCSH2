using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class FunctionCallStatement : Statement
    {
        public string Identifier { get; set; }
        public TokenType Type { get; set; }
        public List<Expression> Arguments { get; set; }

        public object? returnedValue { get; set; } //value function returns after calling

        public FunctionCallStatement(Token identifier)
        {
            if (identifier == null || identifier.Type != TokenType.Ident)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);
            Identifier = identifier.Value;
            Arguments = new List<Expression>();
        }

        public FunctionCallStatement(Token identifier, List<Expression> arguments)
        {
            if (identifier == null || identifier.Type != TokenType.Ident)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);
            Identifier = identifier.Value;
            Arguments = arguments;
        }

        public override void Evaluate(ExecutionCntxt context)
        {
            returnedValue = context.ProgramContext.Call(Identifier, context, Arguments);
            Type = context.GetFunction(Identifier).ReturnType;
        }
    }
}
