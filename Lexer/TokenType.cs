using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemPrace_BTEJA_BCSH2
{
    public enum TokenType
    {
        Let, Function, If, Else, While, Return,
        Int, Double, String, Boolean, Void,
        Equal_Equal, Not_Equal, Greater, Less, Greater_Equal, Less_Equal,
        Plus, Minus, Slash, Star,
        Dot, Colon, Semi_Colon, Comma, Equal,
        Left_Bracket, Right_Bracket, Left_Curly_Bracket, Right_Curly_Bracket,
        True, False,
        Ident, Number, Real_Number, String_Value
    }
}
