using System.Collections.Generic;
using System.Globalization;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Parsers
{
    internal class NumberTokenParser : ITokenParser
    {
        public List<Token> TryParse(string expression, CultureInfo cultureInfo)
        {
            var tokens = new List<Token>();
            if (decimal.TryParse(expression, NumberStyles.AllowDecimalPoint, cultureInfo, out var number))
            {
                tokens.Add(new Token(number, cultureInfo));
            }

            return tokens;
        }
    }
}