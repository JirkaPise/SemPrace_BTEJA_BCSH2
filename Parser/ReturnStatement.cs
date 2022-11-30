using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class ReturnStatement : Statement
    {

        public Expression? Expression { get; set; }

        public ReturnStatement(Expression? expression)
        {
            Expression = expression;
        }

        public override object? Evaluate(ExecutionCntxt context)
        {
            if(Expression != null)
                return Expression?.Evaluate(context);
            return this;
        }

    }
}
