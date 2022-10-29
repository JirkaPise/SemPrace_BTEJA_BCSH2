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

        //TODO
        public Program Parse(List<Token> tokens)
        {
            Tokens = tokens;
            return new Program();
        }

        public Condition ReadCondition()
        {
            Condition condition;

            Expression expression1 = ReadExpression();
            Token token = ReadToken();
            Expression expression2 = ReadExpression();
            condition = new Condition(expression1, token, expression2);

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
                return new Factor(token);
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
    }
}
