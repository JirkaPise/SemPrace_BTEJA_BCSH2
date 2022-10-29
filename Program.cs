using SemPrace_BTEJA_BCSH2;

string input;

using (StreamReader sr = new StreamReader("input.txt")) 
{
    input = sr.ReadToEnd();
}

Lexer lexer = new Lexer();
List<Token> tokens = lexer.ScanTokens(input);

foreach (Token token in tokens)
{
    Console.WriteLine(token);
}
