using SemPrace_BTEJA_BCSH2.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{

    public class ExecutionCntxt
    {
        public ProgramContext ProgramContext { get; set; }
        public Variables Variables { get; set; }
        private Variables Arguments { get; set; } //only for functions. After function is finished this will be empty again
        public ExecutionCntxt? GlobalExecutionContext { get; set; }

        public ExecutionCntxt(ProgramContext programContext, ExecutionCntxt? globalExecutionContext)
        {
            ProgramContext = programContext;
            Variables = new Variables();
            Arguments = new Variables();
            GlobalExecutionContext = globalExecutionContext;
        }

        public void AddArguments(List<Argument> arguments)
        {
            foreach (Argument argument in arguments)
            {
                Arguments.SetVar(argument.Identifier, argument.Value, argument.Type);
            }
        }

        public void ClearArguments()
        {
            Arguments = new Variables();
        }

        public Function GetFunction(string name)
        {
            foreach (Function function in ProgramContext.Functions)
            {
                if (function.Identifier == name)
                    return function;
            }
            return null;
            //throw new Exception("Function " + name + " not found in this context");
        }

        public Var GetVar(string name)
        {

            if (Variables.GetVar(name) != null)
                return Variables.GetVar(name);

            if (GlobalExecutionContext != null && GlobalExecutionContext.Variables.GetVar(name) != null)
                return GlobalExecutionContext.Variables.GetVar(name);

            if (Arguments.GetVar(name) != null)
                return Arguments.GetVar(name);

            if (GlobalExecutionContext != null && GlobalExecutionContext.Arguments.GetVar(name) != null)
                return GlobalExecutionContext.Arguments.GetVar(name);
            return null;


            //throw new Exception("Variable " + name + " not found in this context");
        }

        public object GetValue(string VariableName)
        {
            object? value = Variables.GetVarValue(VariableName);
            if (value == null && GlobalExecutionContext != null)
                value = GlobalExecutionContext.Variables.GetVarValue(VariableName);
            if (value == null)
                value = Arguments.GetVarValue(VariableName);
            if (value == null && GlobalExecutionContext != null)
                value = GlobalExecutionContext.Arguments.GetVarValue(VariableName);
            if (value == null)
                throw new Exception("Variable " + VariableName + " not found in this context");

            return value;
        }

    }
}
