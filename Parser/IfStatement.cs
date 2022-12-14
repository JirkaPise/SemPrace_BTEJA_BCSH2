using SemPrace_BTEJA_BCSH2.Interpreter;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class IfStatement : Statement
    {
        Condition Condition { get; set; }
        List<Statement> IfStatements { get; set; } // do if condition is true
        List<Statement>? ElseStatements { get; set; } // do if condition is false

        public IfStatement(Condition condition, List<Statement> ifStatements)
        {
            Condition = condition;
            IfStatements = ifStatements;
        }

        public IfStatement(Condition condition, List<Statement> ifStatements, List<Statement>? elseStatements) : this(condition, ifStatements)
        {
            ElseStatements = elseStatements;
        }

        public override object? Evaluate(ExecutionCntxt context)
        {
            if (Condition.Evaluate(context))
            {
                foreach (Statement s in IfStatements)
                {
                    object? o = s.Evaluate(context);
                    if (o != null)
                        return o;
                }
            }
            else
            {
                if (ElseStatements != null)
                {
                    foreach (Statement s in ElseStatements) 
                    {    
                        object? o = s.Evaluate(context); 
                        if(o != null)
                            return o;
                    }
                }
            }
            return null;
        }
    }
}