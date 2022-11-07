using SemPrace_BTEJA_BCSH2.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public class Function
    {
        public string Identifier { get; set; }
        public List<Argument> Arguments { get; set; }
        public TokenType ReturnType { get; set; }
        public List<Statement> Statements { get; set; }
        public Expression? ReturnExpression { get; set; }

        public Function(string identifier, List<Argument> arguments, Token returnType, List<Statement> statements, Expression? returnExpression)
        {
            Identifier = identifier;
            Arguments = arguments;
            ReturnType = returnType.Type;
            Statements = statements;
            ReturnExpression = returnExpression;
        }

        public object? Call(ExecutionCntxt context, List<Expression>? argumentValues)
        {
            LinkArgumetns(context, argumentValues);
            context.AddArguments(Arguments);
            foreach (Statement s in Statements)
            {
                s.Evaluate(context);
            }

            object? returnValue = null;
            if (ReturnExpression != null)
            {
                returnValue = ReturnExpression.Evaluate(context);
                Variables.CheckType(returnValue, Variables.ConvertToValue(ReturnType));
            }
            context.ClearArguments();
            return returnValue;
        }

        // set Values to arguments
        private void LinkArgumetns(ExecutionCntxt context, List<Expression> argumentValues)
        {
            if (Arguments.Count != argumentValues.Count)
                throw new Exception("Argument count is not same as argumentValues");
            for (int i = 0; i < Arguments.Count; i++)
            {
                object? value = argumentValues[i].Evaluate(context);
                Arguments[i].Type = Variables.ConvertToValue(Arguments[i].Type);
                Variables.CheckType(value, Arguments[i].Type);
                Arguments[i].Value = value;
            }
        }
    }
}
