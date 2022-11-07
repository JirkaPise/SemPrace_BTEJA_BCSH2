
using SemPrace_BTEJA_BCSH2.Interpreter;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class FunctionDeclarationStatement : Statement
    {
        public string Identifier { get; set; }
        public List<Argument> Arguments { get; set; }
        public Token ReturnType { get; set; }
        public List<Statement> Statements { get; set; }
        public Expression? ReturnExpression { get; set; }

        public FunctionDeclarationStatement(Token identifier,
            List<Argument> arguments, Token returnType,
            List<Statement> statements, Expression? returnExpression)
        {
            if (identifier == null || identifier.Type != TokenType.Ident || identifier.Value == null)
                Parser.UnexpectedTokenError(identifier, TokenType.Ident);

            Identifier = identifier.Value;
            Arguments = arguments;
            if (!Parser.IsReturnableType(returnType) && returnType.Type != TokenType.Void)
                throw new Exception("Return type must be type int or double or string or boolean or void" +
                    "\nError on line: " + returnType.Line);
            ReturnType = returnType;
            Statements = statements;
            ReturnExpression = returnExpression;
        }

        public override void Evaluate(ExecutionCntxt context)
        {
            if (context.GetFunction(Identifier) != null)
                throw new Exception("Funkce " + Identifier + " already exists");

            context.ProgramContext.Functions.Add(new Function(Identifier,
                Arguments, ReturnType, Statements, ReturnExpression));
        }
    }
}