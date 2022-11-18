using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public TokenType Type { get; set; }


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

        public object? Evaluate(ExecutionCntxt context)
        {
            object? value = Term1.Evaluate(context);
            Type = Term1.Type;

            if (Type == TokenType.String_Value)
            {
                object s = null;
                if (Term2 != null && Token2 != null && Token2.Type == TokenType.Plus)
                    return (string)value + (string)Term2.Evaluate(context);
                else if (ExpressionPrev != null && Token1 != null && Token1.Type == TokenType.Plus)
                    return (string)ExpressionPrev.Evaluate(context) + (string)value;
                else return value;

            }

            if (Type != TokenType.Number && Type != TokenType.Real_Number)
                return value;

            if (Token1 != null && Token1.Type == TokenType.Minus)
            {
                if (Type == TokenType.Real_Number)
                    value = -(double)value;
                if (Type == TokenType.Number)
                    value = -(int)value;
            }

            if (Term2 != null)
            {
                if (Token2.Type == TokenType.Plus)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)value + (double)Term2.Evaluate(context);
                    if (Type == TokenType.Number)
                        value = (int)value + (int)Term2.Evaluate(context);
                }
                else if (Token2.Type == TokenType.Minus)
                {
                    if (Type == TokenType.Real_Number)
                        value = (double)value - (double)Term2.Evaluate(context);
                    if (Type == TokenType.Number)
                        value = (int)value - (int)Term2.Evaluate(context);
                }
            }
            else if (ExpressionPrev != null)
            {
                if (Type == TokenType.Real_Number)
                    value = (double)ExpressionPrev.Evaluate(context) + (double)value;
                if (Type == TokenType.Number)
                    value = (int)ExpressionPrev.Evaluate(context) + (int)value;

            }

            if (Type == TokenType.Number)
                return (int)value;

            return (double)value;

        }

    }
}
