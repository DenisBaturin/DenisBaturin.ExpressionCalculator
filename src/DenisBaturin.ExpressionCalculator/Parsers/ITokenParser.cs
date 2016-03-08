using System.Collections.Generic;
using System.Globalization;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Parsers
{
    internal interface ITokenParser
    {
        List<Token> TryParse(string expression, CultureInfo cultureInfo);
    }
}