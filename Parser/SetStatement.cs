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


    }
}
