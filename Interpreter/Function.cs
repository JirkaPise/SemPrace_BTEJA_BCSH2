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

        public Function(string identifier, List<Argument> arguments, Token returnType, List<Statement> statements)
        {
            Identifier = identifier;
            Arguments = arguments;
            ReturnType = returnType.Type;
            Statements = statements;
        }

        public object? Call(ExecutionCntxt globalContext, List<Expression>? argumentValues)
        {
            ExecutionCntxt context = new ExecutionCntxt(globalContext.ProgramContext, globalContext);
            LinkArgumetns(context, argumentValues);
            context.AddArguments(Arguments);
            object? returnValue = null;
            foreach (Statement s in Statements)
            {
                object? o = s.Evaluate(context);
                if (o != null)
                {
                    if (o.GetType() == typeof(ReturnStatement) && ReturnType != TokenType.Void)
                        throw new Exception("Cannot use empty retrun in " + ReturnType + " function. " + Identifier);
                    if (o.GetType() != typeof(ReturnStatement) && ReturnType == TokenType.Void)
                        throw new Exception("Cannot return value from VOID function " + Identifier);

                    if (o.GetType() != typeof(ReturnStatement))
                        returnValue = o;
                    else
                        return null;

                    Variables.CheckType(returnValue, Variables.ConvertToValue(ReturnType));
                    break;
                }
            }

            if (returnValue == null && ReturnType != TokenType.Void)
                throw new Exception(ReturnType + " function " + Identifier + " must return some value");


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
