using SemPrace_BTEJA_BCSH2.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public class ProgramContext
    {
        public List<Function> Functions { get; set; }
        public List<string> BuildInFunctions { get; set; }

        public ProgramContext()
        {
            Functions = new List<Function>();
            BuildInFunctions = new List<string>();
            AddBuildInFunctions();
        }

        public object? Call(string name, ExecutionCntxt globalContext, List<Expression> argumentValues)
        {

            ExecutionCntxt context = new ExecutionCntxt(globalContext.ProgramContext, globalContext);
            if (BuildInFunctions.Contains(name))
            {
                return CallBuilInFunction(name, context, argumentValues);
            }

            foreach (Function function in Functions)
            {
                if (function.Identifier == name)
                {
                    return function.Call(context, argumentValues);
                }
            }
            throw new Exception("Function " + name + " not found");
        }

        private object CallBuilInFunction(string name, ExecutionCntxt context, List<Expression> argumentValues)
        {
            if (name == "log")
            {
                if (argumentValues.Count != 1)
                    throw new Exception("Invalid amount of arguments in log function");
                object input = argumentValues[0].Evaluate(context);
                Console.WriteLine(input);
            }
            else if (name == "readLine")
            {
                if (argumentValues.Count != 0)
                    throw new Exception("Invalid amount of arguments in readLine function");
                return Console.ReadLine();
            }
            else if (name == "readFile")
            {
                if (argumentValues.Count != 1)
                    throw new Exception("Invalid amount of arguments in readFile function");
                object input = argumentValues[0].Evaluate(context);
                if (input.GetType() != typeof(string))
                    throw new Exception("Invalid input path string in readFile function");
                using (StreamReader streamReader = new StreamReader((string)input))
                {
                    return streamReader.ReadToEnd();
                }
                throw new Exception("Could not read file");
            }
            else if (name == "writeFile")
            {
                if (argumentValues.Count != 2)
                    throw new Exception("Invalid amount of arguments in readFile function");
                object path = argumentValues[0].Evaluate(context);
                object s = argumentValues[1].Evaluate(context);
                if (path.GetType() != typeof(string))
                    throw new Exception("Invalid input path string in writeFile function");
                if (s.GetType() != typeof(string))
                    throw new Exception("Invalid input to write to file in writeFile function");

                using (StreamWriter streamWriter = new StreamWriter((string)path))
                {
                    streamWriter.WriteLine((string?)s);
                }

            }
            else if (name == "toInt")
            {
                if (argumentValues.Count != 1)
                    throw new Exception("Invalid amount of arguments in readFile function");
                object input = argumentValues[0].Evaluate(context);
                if (input.GetType() == typeof(double))
                {
                    return Convert.ToInt32((double)input);
                }
                else if (input.GetType() == typeof(string))
                {
                    return int.Parse((string)input);
                }
                else if (input.GetType() == typeof(int))
                {
                    return input;
                }
                else
                {
                    throw new Exception("Cannot convert " + input.GetType().Name + " to int");
                }
            }
            else if (name == "toDouble")
            {
                if (argumentValues.Count != 1)
                    throw new Exception("Invalid amount of arguments in readFile function");
                object input = argumentValues[0].Evaluate(context);
                if (input.GetType() == typeof(int))
                {
                    return Convert.ToDouble(input);
                }
                else if (input.GetType() == typeof(string))
                {
                    return double.Parse((string)input);
                }
                else if (input.GetType() == typeof(double))
                {
                    return input;
                }
                else
                {
                    throw new Exception("Cannot convert " + input.GetType().Name + " to double");
                }
            }
            else if (name == "toString")
            {
                if (argumentValues.Count != 1)
                    throw new Exception("Invalid amount of arguments in readFile function");
                object input = argumentValues[0].Evaluate(context);
                if (input.GetType() == typeof(double) || input.GetType() == typeof(int) || input.GetType() == typeof(bool))
                {
                    return input.ToString();
                }
                else
                {
                    throw new Exception("Cannot convert " + input.GetType().Name + " to string");
                }
            }
            return null;
        }

        private void AddBuildInFunctions()
        {
            Functions.Add(new Function("log", new List<Argument>(), new Token(TokenType.Void, -1), null, null));
            BuildInFunctions.Add("log");
            Functions.Add(new Function("readLine", new List<Argument>(), new Token(TokenType.String, -1), null, null));
            BuildInFunctions.Add("readLine");
            Functions.Add(new Function("writeFile", new List<Argument>(), new Token(TokenType.Void, -1), null, null));
            BuildInFunctions.Add("writeFile");
            Functions.Add(new Function("readFile", new List<Argument>(), new Token(TokenType.String, -1), null, null));
            BuildInFunctions.Add("readFile");
            Functions.Add(new Function("toInt", new List<Argument>(), new Token(TokenType.Int, -1), null, null));
            BuildInFunctions.Add("toInt");
            Functions.Add(new Function("toString", new List<Argument>(), new Token(TokenType.String, -1), null, null));
            BuildInFunctions.Add("toString");
            Functions.Add(new Function("toDouble", new List<Argument>(), new Token(TokenType.Double, -1), null, null));
            BuildInFunctions.Add("toDouble");
        }

    }

}
