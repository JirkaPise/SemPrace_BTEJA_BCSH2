using SemPrace_BTEJA_BCSH2.Interpreter;
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
        public TokenType Type { get; set; }

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

        public object? Evaluate(ExecutionCntxt context)
        {
            object? value = Factor1.Evaluate(context);
            Type = Factor1.Type;

            if (Type != TokenType.Number && Type != TokenType.Real_Number)
                return value;

            if (Factor2 != null)
            {
                if (Token.Type == TokenType.Slash)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)value / (double)Factor2.Evaluate(context);
                    if (Type == TokenType.Number)
                        value = (int)value / (int)Factor2.Evaluate(context);
                }
                else if (Token.Type == TokenType.Star)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)value * (double)Factor2.Evaluate(context);
                    if (Type == TokenType.Number)
                        value = (int)value * (int)Factor2.Evaluate(context);
                }
            }
            else if (TermPrev != null)
            {
                if (Token.Type == TokenType.Slash)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)TermPrev.Evaluate(context) / (double)value;
                    if (Type == TokenType.Number)
                        value = (int)TermPrev.Evaluate(context) / (int)value;
                }
                else if (Token.Type == TokenType.Star)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)TermPrev.Evaluate(context) * (double)value;
                    if (Type == TokenType.Number)
                        value = (int)TermPrev.Evaluate(context) * (int)value;
                }
            }

            if (Factor1.Type == TokenType.Number)
                return (int)value;
            return value;
        }

    }
}
