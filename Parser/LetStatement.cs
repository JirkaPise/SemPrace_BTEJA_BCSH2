using SemPrace_BTEJA_BCSH2.Interpreter;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class LetStatement : Statement
    {
        public string Identifier { get; set; }
        public TokenType Type { get; set; }
        Expression? Expression { get; set; }

        public LetStatement(Token identifier, Token type)
        {
            if (identifier == null || identifier.Type != TokenType.Ident || identifier.Value == null)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);

            Identifier = identifier.Value;

            if (!Parser.IsReturnableType(type))
                throw new Exception("Argument must be type int or double or string or boolean" +
                    "\nError on line: " + type.Line);

            Type = type.Type;

        }

        public LetStatement(Token identifier, Token type, Expression? expression) : this(identifier, type)
        {
            Expression = expression;
        }

        public override void Evaluate(ExecutionCntxt context)
        {
            if (context.GetVar(Identifier) != null)
            {
                throw new Exception("Variable " + Identifier + " already exists");
            }
            if (Expression == null)
            {
                context.Variables.SetVar(Identifier, null, Type);
            }
            else
            {
                object? value = Expression.Evaluate(context);
                Variables.CheckType(value, Variables.ConvertToValue(Type));
                context.Variables.SetVar(Identifier, value, Variables.ConvertToValue(Type));
            }
        }

    }
}