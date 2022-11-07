using SemPrace_BTEJA_BCSH2.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Parser
{
    public class Statement
    {
        public virtual void Evaluate(ExecutionCntxt context) { }
    }
}
