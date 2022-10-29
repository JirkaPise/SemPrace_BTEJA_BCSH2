using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Term
    {
        public Term? TermPrev { get; set; } // previous term if there is repetetive * or / in one term
        public Factor Factor1 { get; set; } //main factor
        public Token? Token { get; set; } // * or /
        public Factor? Factor2 { get; set; } //secondary factor

        public Term(Factor factor1)
        {
            Factor1 = factor1;
        }

        public Term(Factor factor1, Token token, Factor factor2)
        {
            if (token.Type == TokenType.Star || token.Type == TokenType.Slash)
            {
                Factor1 = factor1;
                Token = token;
                Factor2 = factor2;
            }
            else
            {
                Parser.UnexpectedTokenError(token, TokenType.Star);
            }
        }


        public Term(Term term, Token token, Factor factor)
        {
            if (token.Type == TokenType.Star || token.Type == TokenType.Slash)
            {
                TermPrev = term;
                Token = token;
                Factor1 = factor;
            }
            else
            {
                Parser.UnexpectedTokenError(token, TokenType.Star);
            }
        }



    }
}
