
using SemPrace_BTEJA_BCSH2;
using SemPrace_BTEJA_BCSH2.Parser;

string input;

using (StreamReader sr = new StreamReader("input.txt")) 
{
    input = sr.ReadToEnd();
}

Lexer lexer = new Lexer();
List<Token> tokens = lexer.ScanTokens(input);

Parser parser = new Parser();
parser.Parse(tokens);

Condition ex = parser.ReadCondition();
Console.WriteLine();
