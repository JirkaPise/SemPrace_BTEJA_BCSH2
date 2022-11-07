using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2.Interpreter
{
    public class Variables
    {
        private List<Var> Vars { get; set; }

        public Variables()
        {
            Vars = new List<Var>();
        }

        public object? GetVarValue(string name)
        {
            foreach (var v in Vars)
                if (v.Name == name)
                    return v.Value;
            return null;
        }

        public void SetVar(string name, object? value, TokenType type)
        {
            CheckType(value, type);
            Vars.Add(new Var(name, value, type));
        }

        public Var? GetVar(string name)
        {
            foreach (var v in Vars)
                if (v.Name == name)
                    return v;
            return null;
        }

        public static void CheckType(object? value, TokenType type)
        {
            if (type == TokenType.Number && value.GetType() != typeof(int))
                throw new Exception("Cannot set this value " + value.GetType().Name + " to int variable");
            if (type == TokenType.Real_Number && value.GetType() != typeof(double))
                throw new Exception("Cannot set this value " + value.GetType().Name + " to double variable");
            if (type == TokenType.Boolean && value.GetType() != typeof(bool))
                throw new Exception("Cannot set this value " + value.GetType().Name + "  to boolean variable");
            if (type == TokenType.String_Value && value.GetType() != typeof(string))
                throw new Exception("Cannot set this value " + value.GetType().Name + "  to string variable");
        }

        public static TokenType ConvertToValue(TokenType input)
        {
            if (input == TokenType.Number || input == TokenType.Real_Number ||
                input == TokenType.Boolean || input == TokenType.String_Value)
                return input;

            if (input == TokenType.Int)
                return TokenType.Number;
            if (input == TokenType.Double)
                return TokenType.Real_Number;
            if (input == TokenType.String)
                return TokenType.String_Value;
            if (input == TokenType.Boolean)
                return TokenType.Boolean;

            throw new Exception("Unexpected type - " + input + " cannot be converted to value");
        }

    }
}
