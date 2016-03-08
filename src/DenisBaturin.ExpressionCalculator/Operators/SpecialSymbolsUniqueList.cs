using System;
using System.Collections.Generic;
using System.Linq;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Operators
{
    internal class SpecialSymbolsUniqueList : List<SpecialSymbol>
    {
        public new void Add(SpecialSymbol item)
        {
            var isThereSuchOperators = this.Any(
                ss =>
                    ss.StringView.Equals(item.StringView, StringComparison.InvariantCultureIgnoreCase)
                    ||
                    ss.Type == item.Type
                );

            if (isThereSuchOperators)
            {
                throw new ArgumentException(
                    $"An error occurred while adding special symbol: special symbol [{item.StringView} - {item.Type}] has already declared.");
            }

            if (item.StringView.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException(
                    $"An error occurred while adding special symbol: special symbol [{item.StringView} - {item.Type}] has whitespace in the StringView.");
            }

            if (item.StringView.Length !=1)
            {
                throw new ArgumentException(
                    $"An error occurred while adding special symbol: special symbol [{item.StringView} - {item.Type}] must be only one character in the StringView.");
            }

            base.Add(item);
        }
    }
}