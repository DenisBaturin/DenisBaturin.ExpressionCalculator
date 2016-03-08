using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DenisBaturin.ExpressionCalculator.Operators;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Parsers
{
    internal class SpecialSymbolTokenParser : ITokenParser
    {
        private readonly SpecialSymbolsUniqueList _specialSymbols;

        public SpecialSymbolTokenParser (SpecialSymbolsUniqueList specialSymbols)
        {
            _specialSymbols = specialSymbols;
        }

        public List<Token> TryParse(string expression, CultureInfo cultureInfo)
        {
            var tokens = _specialSymbols
                .Where(ss => ss.StringView == expression)
                .Select(ss => new Token(ss, cultureInfo))
                .ToList();

            return tokens;
        }
    }
}