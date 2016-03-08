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
            decimal number;
            if (decimal.TryParse(expression, NumberStyles.AllowDecimalPoint, cultureInfo, out number))
            {
                tokens.Add(new Token(number, cultureInfo));
            }

            return tokens;
        }
    }
}