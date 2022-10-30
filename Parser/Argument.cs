namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Argument
    {
        public string Identifier { get; set; }
        public Token Token { get; set; }

        public Argument(Token identifier, Token token)
        {
            if (identifier == null || identifier.Type != TokenType.Ident || identifier.Value == null)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);

            Identifier = identifier.Value;
            Token = token;

            if (!Parser.IsReturnableType(Token))
                throw new Exception("Argument must be type int or double or string or boolean" +
                    "\nError on line: " + token.Line);
        }
    }
}