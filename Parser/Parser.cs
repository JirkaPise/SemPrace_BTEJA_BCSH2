using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Parser
    {
        private List<Token> Tokens { get; set; }

        public Program Parse(List<Token> tokens)
        {
            Tokens = tokens;
            return new Program();
        }

        public Statement ReadStatement()
        {
            switch (PeekToken().Type)
            {
                case TokenType.Let: return ReadLetStatement();
                case TokenType.Ident: return ReadIdentStatement();
                case TokenType.Function: return ReadFunctionDeclarationStatement();
                case TokenType.If: return ReadIfStatement();
                case TokenType.While: return ReadWhileStatement();
            }
            UnexpectedTokenError(PeekToken());
            return null;
        }

        private LetStatement ReadLetStatement()
        {
            ReadToken(); // let
            Token ident = ReadToken();
            CheckToken(ReadToken(), TokenType.Colon);
            Token type = ReadToken();
            LetStatement statement = null;
            if (PeekToken().Type == TokenType.Semi_Colon)
            {
                statement = new LetStatement(ident, type);
            }
            else if (PeekToken().Type == TokenType.Equal)
            {
                ReadToken(); // =
                statement = new LetStatement(ident, type, ReadExpression());
            }
            else
            {
                UnexpectedTokenError(ReadToken(), TokenType.Semi_Colon);
            }
            CheckToken(ReadToken(), TokenType.Semi_Colon);
            return statement;

        }

        private Statement ReadIdentStatement()
        {
            Token ident = ReadToken();
            Statement statement;
            if (PeekToken().Type == TokenType.Left_Bracket)
                statement = ReadFunctionCallStatement(ident);
            else if (PeekToken().Type == TokenType.Equal)
                statement = ReadSetStatement(ident);
            else
                UnexpectedTokenError(ReadToken()); return null;

            CheckToken(ReadToken(), TokenType.Semi_Colon); // ;
            return statement;
        }

        private SetStatement ReadSetStatement(Token ident)
        {
            ReadToken(); // =
            Expression expression = ReadExpression();
            return new SetStatement(ident, expression);
        }

        private FunctionCallStatement ReadFunctionCallStatement(Token ident)
        {
            ReadToken(); // (
            List<Expression> arguments = new List<Expression>();
            while (PeekToken().Type != TokenType.Right_Bracket)
            {
                arguments.Add(ReadExpression());
                if (PeekToken().Type != TokenType.Right_Bracket)
                    CheckToken(ReadToken(), TokenType.Comma);
            }
            CheckToken(ReadToken(), TokenType.Right_Bracket); // )
            return new FunctionCallStatement(ident, arguments);

        }

        private FunctionDeclarationStatement ReadFunctionDeclarationStatement()
        {
            ReadToken(); // function

            Token identifier = ReadToken(); // ident
            CheckToken(ReadToken(), TokenType.Left_Bracket); // (
            List<Argument> arguments = new List<Argument>(); // x: string...
            while (PeekToken().Type != TokenType.Right_Bracket)
            {
                Token argumentIdent = ReadToken();
                CheckToken(ReadToken(), TokenType.Colon);
                Token type = ReadToken();
                arguments.Add(new Argument(argumentIdent, type));
                if (PeekToken().Type != TokenType.Right_Bracket)
                    CheckToken(ReadToken(), TokenType.Comma);
            }
            CheckToken(ReadToken(), TokenType.Right_Bracket); // )

            CheckToken(ReadToken(), TokenType.Colon); // :
            Token returnType = ReadToken(); // int

            CheckToken(ReadToken(), TokenType.Left_Curly_Bracket); // {
            List<Statement> statements = new List<Statement>();
            while (PeekToken() != null
                && PeekToken().Type != TokenType.Return && PeekToken().Type != TokenType.Right_Curly_Bracket)
            {
                if (PeekToken().Type == TokenType.Right_Curly_Bracket && returnType.Type != TokenType.Void)
                    throw new Exception("Missing return in function");
                statements.Add(ReadStatement());
            }

            if (returnType.Type != TokenType.Void)
            {
                CheckToken(ReadToken(), TokenType.Return); // return
                Expression returnExpression = ReadExpression(); // 4
                CheckToken(ReadToken(), TokenType.Semi_Colon); // ;
                CheckToken(ReadToken(), TokenType.Right_Curly_Bracket); // }
                return new FunctionDeclarationStatement(identifier, arguments, returnType, statements, returnExpression);
            }
            else
            {
                CheckToken(ReadToken(), TokenType.Right_Curly_Bracket); // }
                return new FunctionDeclarationStatement(identifier, arguments, returnType, statements, null);
            }

        }


        public IfStatement ReadIfStatement()
        {
            ReadToken(); // read if token

            CheckToken(ReadToken(), TokenType.Left_Bracket);
            Condition condition = ReadCondition();
            CheckToken(ReadToken(), TokenType.Right_Bracket);
            CheckToken(ReadToken(), TokenType.Left_Curly_Bracket);

            List<Statement> ifStatements = new List<Statement>();
            while (PeekToken() != null && PeekToken().Type != TokenType.Right_Curly_Bracket)
                ifStatements.Add(ReadStatement());

            CheckToken(ReadToken(), TokenType.Right_Curly_Bracket);
            if (PeekToken() == null || PeekToken().Type != TokenType.Else)
                return new IfStatement(condition, ifStatements);

            ReadToken(); //reads else token
            CheckToken(ReadToken(), TokenType.Left_Curly_Bracket);

            List<Statement> elseStatements = new List<Statement>();
            while (PeekToken() != null && PeekToken().Type != TokenType.Right_Curly_Bracket)
                elseStatements.Add(ReadStatement());

            CheckToken(ReadToken(), TokenType.Right_Curly_Bracket);
            return new IfStatement(condition, ifStatements, elseStatements);

        }

        public WhileStatement ReadWhileStatement()
        {
            ReadToken(); // read while token

            CheckToken(ReadToken(), TokenType.Left_Bracket);
            Condition condition = ReadCondition();
            CheckToken(ReadToken(), TokenType.Right_Bracket);
            CheckToken(ReadToken(), TokenType.Left_Curly_Bracket);

            List<Statement> statements = new List<Statement>();
            while (PeekToken() != null && PeekToken().Type != TokenType.Right_Curly_Bracket)
            {
                statements.Add(ReadStatement());
            }

            CheckToken(ReadToken(), TokenType.Right_Curly_Bracket);

            return new WhileStatement(condition, statements);
        }

        public Condition ReadCondition()
        {
            Condition condition;
            Expression expression1 = ReadExpression();
            if (PeekToken().Type == TokenType.Right_Bracket)
            {
                condition = new Condition(expression1.Term1.Factor1);
            }
            else
            {
                Token token = ReadToken();
                Expression expression2 = ReadExpression();
                condition = new Condition(expression1, token, expression2);
            }
            return condition;
        }
        public Expression ReadExpression()
        {
            Token? token1 = null;
            if (PeekToken().Type == TokenType.Plus || PeekToken().Type == TokenType.Minus)
                token1 = ReadToken();

            Term term1 = ReadTerm();
            Token? token = PeekToken();
            if (token == null || token.Type != TokenType.Plus && token.Type != TokenType.Minus)
                return new Expression(token1, term1);
            ReadToken();
            Term term2 = ReadTerm();
            Expression expression = new Expression(token1, term1, token, term2);

            while (PeekToken() != null)
            {
                if (PeekToken().Type == TokenType.Plus || PeekToken().Type == TokenType.Minus)
                {
                    token = ReadToken();
                    term2 = ReadTerm();
                    expression = new Expression(expression, token, term2);
                }
                else
                {
                    break;
                }
            }

            return expression;
        }

        private Term ReadTerm()
        {

            Factor factor1 = ReadFactor();
            Token? token = PeekToken();
            if (token == null || token.Type != TokenType.Star && token.Type != TokenType.Slash)
                return new Term(factor1);
            ReadToken();
            Factor factor2 = ReadFactor();
            Term term = new Term(factor1, token, factor2);

            while (PeekToken() != null)
            {
                if (PeekToken().Type == TokenType.Star || PeekToken().Type == TokenType.Slash)
                {
                    token = ReadToken();
                    factor2 = ReadFactor();
                    term = new Term(term, token, factor2);
                }
                else
                {
                    break;
                }
            }

            return term;
        }

        private Factor ReadFactor()
        {
            if (PeekToken().Type != TokenType.Left_Bracket)
            {
                Token token = ReadToken();
                if (token.Type == TokenType.Ident)
                {
                    if (PeekToken().Type != TokenType.Left_Bracket)
                        return new Factor(token);
                    else
                        return new Factor(ReadFunctionCallStatement(token));
                }
                else
                {
                    return new Factor(token);
                }
            }
            else
            {
                Token leftBracket = ReadToken();
                Expression expression = ReadExpression();
                Token rightBracket = ReadToken();
                return new Factor(leftBracket, expression, rightBracket);
            }
        }


        private Token? PeekToken()
        {
            if (Tokens.Count == 0)
                return null;
            return Tokens[0];
        }

        private Token? ReadToken()
        {
            if (Tokens.Count == 0)
                return null;
            Token token = Tokens[0];
            Tokens.Remove(token);
            return token;
        }

        public void CheckToken(Token? token, TokenType expectedType)
        {

            if (token == null || token.Type != expectedType)
                UnexpectedTokenError(token, expectedType);
        }

        public static void UnexpectedTokenError(Token? unexpected)
        {
            if (unexpected == null)
                throw new Exception("Token is null");
            throw new Exception("Unexpected token: '" + unexpected.Type.ToString() + "' on line "
                + unexpected.Line);
        }

        public static void UnexpectedTokenError(Token? unexpected, TokenType expected)
        {
            if (unexpected == null)
                throw new Exception("Token is null");
            throw new Exception("Unexpected token: '" + unexpected.Type.ToString() + "' on line "
                + unexpected.Line + "\n" + "Expected: " + expected);
        }

        public static bool IsReturnableType(Token token)
        {
            return token != null && token.Type == TokenType.Int ||
                token.Type == TokenType.Double || token.Type == TokenType.String ||
                token.Type == TokenType.Boolean;
        }
    }
}
