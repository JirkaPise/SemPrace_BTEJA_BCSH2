using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Condition
    {
        public Expression? Expression1 { get; set; }
        public Token? Token { get; set; }
        public Expression? Expression2 { get; set; }
        public Factor? Factor { get; set; } // if value is true or false

        public Condition(Factor factor)
        {
            if (factor.Type == TokenType.True ||
                factor.Type == TokenType.False || factor.Type == TokenType.Ident)
                Factor = factor;
            else
                throw new Exception("Not valid factor for condition");
        }

        public Condition(Expression expression1, Token token, Expression expression2)
        {
            if (token.Type == TokenType.Equal_Equal || token.Type == TokenType.Not_Equal ||
                token.Type == TokenType.Less || token.Type == TokenType.Less_Equal ||
                token.Type == TokenType.Greater || token.Type == TokenType.Greater_Equal)
            {
                Expression1 = expression1;
                Token = token;
                Expression2 = expression2;
            }
            else
            {
                Parser.UnexpectedTokenError(token);
            }
        }

    }
}
