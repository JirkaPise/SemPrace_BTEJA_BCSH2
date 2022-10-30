namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class LetStatement : Statement
    {
        public string Identifier { get; set; }
        public Token Type { get; set; }
        Expression? Expression { get; set; }

        public LetStatement(Token identifier, Token type)
        {
            if (identifier == null || identifier.Type != TokenType.Ident || identifier.Value == null)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);

            Identifier = identifier.Value;

            if (!Parser.IsReturnableType(type))
                throw new Exception("Argument must be type int or double or string or boolean" +
                    "\nError on line: " + type.Line);

            Type = type;

        }

        public LetStatement(Token identifier, Token type, Expression? expression) : this(identifier, type)
        {
            Expression = expression;
        }

    }
}