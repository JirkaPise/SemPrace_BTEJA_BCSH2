using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Expression
    {
        public Expression? ExpressionPrev { get; set; } // previous expression if there is multiple + or - in expression
        public Token? Token1 { get; set; } // + or - before term1
        public Term Term1 { get; set; } // main term
        public Token? Token2 { get; set; } // + or - before term2
        public Term? Term2 { get; set; } // secondary term


        public Expression(Token token, Term term1)
        {
            if (token == null || token.Type == TokenType.Plus || token.Type == TokenType.Minus)
            {
                Token1 = token;
                Term1 = term1;
            }
            else
            {
                Parser.UnexpectedTokenError(token);
            }
        }

        public Expression(Token token1, Term term1, Token token2, Term term2)
        {


            if (token2.Type == TokenType.Plus || token2.Type == TokenType.Minus)
            {
                Token1 = token1;
                Term1 = term1;
                Token2 = token2;
                Term2 = term2;
            }
            else
            {
                Parser.UnexpectedTokenError(token2);
            }
        }

        public Expression(Expression expression, Token token, Term term)
        {
            if (token.Type == TokenType.Plus || token.Type == TokenType.Minus)
            {
                ExpressionPrev = expression;
                Token1 = token;
                Term1 = term;
            }
            else
            {
                Parser.UnexpectedTokenError(token);
            }
        }

    }
}
