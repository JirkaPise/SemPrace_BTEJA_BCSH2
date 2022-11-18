using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class FunctionCallStatement : Statement
    {
        public string Identifier { get; set; }
        public TokenType Type { get; set; }
        public List<Expression> ArgumentExpressions { get; set; }

        public object? returnedValue { get; set; } //value function returns after calling

        public FunctionCallStatement(Token identifier)
        {
            if (identifier == null || identifier.Type != TokenType.Ident)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);
            Identifier = identifier.Value;
            ArgumentExpressions = new List<Expression>();
        }

        public FunctionCallStatement(Token identifier, List<Expression> arguments)
        {
            if (identifier == null || identifier.Type != TokenType.Ident)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);
            Identifier = identifier.Value;
            ArgumentExpressions = arguments;
        }

        public override void Evaluate(ExecutionCntxt context)
        {
            returnedValue = context.ProgramContext.Call(Identifier, context, ArgumentExpressions);
            Type = context.GetFunction(Identifier).ReturnType;
        }

    }
}
