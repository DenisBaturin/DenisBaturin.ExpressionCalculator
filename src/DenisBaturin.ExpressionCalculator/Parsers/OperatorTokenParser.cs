using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DenisBaturin.ExpressionCalculator.Operators;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Parsers
{
    internal class OperatorTokenParser : ITokenParser
    {
        private readonly OperatorsUniqueList _operators;

        public OperatorTokenParser(OperatorsUniqueList operators)
        {
            _operators = operators;
        }

        public List<Token> TryParse(string expression, CultureInfo cultureInfo)
        {
            var tokens = _operators
                .Where(o => string.Equals(o.StringView, expression, StringComparison.OrdinalIgnoreCase))
                .Select(o => new Token(o, cultureInfo))
                .ToList();

            return tokens;
        }
    }
}