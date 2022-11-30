using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class SetStatement : Statement
    {
        public string Identifier { get; set; }
        public Expression Expression { get; set; }

        public SetStatement(Token identifier, Expression expression)
        {
            if (identifier == null || identifier.Type != TokenType.Ident)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);
            Identifier = identifier.Value;
            Expression = expression;
        }

        public override object? Evaluate(ExecutionCntxt context)
        {
            Var? v = context.GetVar(Identifier);
            if (v == null)
                throw new Exception("Cannot find variable " + Identifier + " in this context");

            TokenType type = v.Type;
            object? value = Expression.Evaluate(context);
            Variables.CheckType(value, type);
            context.GetVar(Identifier).Value = value;
            return null;
        }


    }
}
