using System;
using System.Collections.Generic;
using System.Linq;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators
{
    internal class OperatorsUniqueList : List<Operator>
    {
        public new void Add(Operator item)
        {
            var isThereSuchOperators = this.Any(
                o =>
                    o.StringView.Equals(item.StringView, StringComparison.InvariantCultureIgnoreCase)
                    &&
                    o.Type == item.Type
                );

            if (isThereSuchOperators)
            {
                throw new ArgumentException(
                    $"An error occurred while adding operator: operator [{item.StringView} - {item.Type}] has already declared.");
            }

            if (item.StringView.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException(
                    $"An error occurred while adding operator: operator [{item.StringView} - {item.Type}] has whitespace in the StringView.");
            }

            base.Add(item);
        }
    }
}