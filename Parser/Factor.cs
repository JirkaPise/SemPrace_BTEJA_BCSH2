using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Factor
    {
        public string? Identifier { get; set; }

        public object? Value { get; set; }

        public TokenType Type { get; set; }

        public Expression? Expression { get; set; }

        public FunctionCallStatement? FunctionCallStatement { get; set; }

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
                if (double.TryParse(token.Value.Replace('.', ','), out number))
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

        public object Evaluate(ExecutionCntxt context)
        {
            if (Value != null)
                return Value;
            if (Expression != null)
                return Expression.Evaluate(context);
            if (Identifier != null)
            {
                if (context.GetVar(Identifier) == null)
                    throw new Exception("Variable " + Identifier + " not found in this context");
                Type = context.GetVar(Identifier).Type;
                return context.GetValue(Identifier);
            }
            if (FunctionCallStatement != null)
            {
                FunctionCallStatement.Evaluate(context);
                Type = Variables.ConvertToValue(FunctionCallStatement.Type);
                return FunctionCallStatement.returnedValue;
            }
            throw new Exception("Not valid factor");
        }

    }
}
