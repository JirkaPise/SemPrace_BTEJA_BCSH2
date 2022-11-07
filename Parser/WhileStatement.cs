using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class WhileStatement : Statement
    {
        public Condition Condition { get; set; }
        public List<Statement> Statements { get; set; }

        public WhileStatement(Condition condition, List<Statement> statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public override void Evaluate(ExecutionCntxt context)
        {
            while (Condition.Evaluate(context))
            {
                foreach (Statement s in Statements)
                {
                    s.Evaluate(context);
                }
            }
        }

    }
}
