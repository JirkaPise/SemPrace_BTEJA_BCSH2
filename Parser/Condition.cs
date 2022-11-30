using SemPrace_BTEJA_BCSH2.Interpreter;
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

        public bool Evaluate(ExecutionCntxt context)
        {
            if (Factor != null)
                return (bool)Factor.Evaluate(context);

            object Value1 = Expression1.Evaluate(context);
            object Value2 = Expression2.Evaluate(context);

            if (Expression1.Type == TokenType.String_Value && Expression2.Type == TokenType.String_Value)
            {
                if (Token.Type == TokenType.Equal_Equal)
                    return (string)Value1 == (string)Value2;
                else if (Token.Type == TokenType.Not_Equal)
                    return (string)Value1 != (string)Value2;

                throw new Exception("Not valid condition on line: " + Token.Line +
                    "\n Cannot compare strings like this");
            }

            double v1 = Convert.ToDouble(Value1);
            double v2 = Convert.ToDouble(Value2);
            if (Token.Type == TokenType.Equal_Equal)
                return v1 == v2;
            else if (Token.Type == TokenType.Not_Equal)
                return v1 != v2;
            else if (Token.Type == TokenType.Less)
                return v1 < v2;
            else if (Token.Type == TokenType.Less_Equal)
                return v1 <= v2;
            else if (Token.Type == TokenType.Greater)
                return v1 > v2;
            else if (Token.Type == TokenType.Greater_Equal)
                return v1 >= v2;

            throw new Exception("Not valid condition on line: " + Token.Line);

        }

    }
}
