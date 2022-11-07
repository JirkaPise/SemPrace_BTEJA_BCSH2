namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Argument
    {
        public string Identifier { get; set; }
        public TokenType Type { get; set; }
        public object? Value { get; set; }

        public Argument(Token identifier, Token token)
        {
            if (identifier == null || identifier.Type != TokenType.Ident || identifier.Value == null)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);

            Identifier = identifier.Value;

            if (!Parser.IsReturnableType(token))
                throw new Exception("Argument must be type int or double or string or boolean" +
                    "\nError on line: " + token.Line);

            Type = token.Type;
        }
    }
}