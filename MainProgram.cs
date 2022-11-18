
using SemPrace_BTEJA_BCSH2;
using SemPrace_BTEJA_BCSH2.Interpreter;
using SemPrace_BTEJA_BCSH2.Parser;

Interpreter interpreter = new Interpreter();
interpreter.Path = "input.txt";
interpreter.Interpret();
