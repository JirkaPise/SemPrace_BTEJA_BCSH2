using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Factor
    {
        public string? Identifier { get; set; }

        public object? Value { get; set; }

        public TokenType Type { get; set; }

        public Expression? Expression { get; set; }

        public FunctionCallStatement FunctionCallStatement { get; set; }

        public Factor(FunctionCallStatement functionCallStatement)
        {
            FunctionCallStatement = functionCallStatement;
        }

        public Factor(Token token)
        {
            if (token == null)
                Parser.UnexpectedTokenError(token);

            Type = token.Type;

            if (Type == TokenType.Number)
            {
                int number;
                if (int.TryParse(token.Value, out number))
                    Value = number;
            }
            else if (Type == TokenType.Real_Number)
            {
                double number;
                if (double.TryParse(token.Value, out number))
                    Value = number;
            }
            else if (Type == TokenType.String_Value)
            {
                Value = token.Value;
            }
            else if (Type == TokenType.Ident)
            {
                Identifier = token.Value;
            }
            else if (Type == TokenType.True)
            {
                Value = true;
            }
            else if (Type == TokenType.False)
            {
                Value = false;
            }
            else
            {
                Parser.UnexpectedTokenError(token);
            }

        }

        public Factor(Token leftBracket, Expression expression, Token rightBracket)
        {
            if (leftBracket.Type != TokenType.Left_Bracket)
                Parser.UnexpectedTokenError(leftBracket, TokenType.Left_Bracket);
            if (rightBracket.Type != TokenType.Right_Bracket)
                Parser.UnexpectedTokenError(rightBracket, TokenType.Right_Bracket);
            Expression = expression;
        }

    }
}
