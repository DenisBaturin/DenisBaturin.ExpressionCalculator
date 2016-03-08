using System.Linq;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator
{
    internal class StringExpressionValidator
    {

        public static ValidationResult Validate(string expression,
            ISpecialSymbol leftBracket, ISpecialSymbol rightBracket)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(expression))
            {
                result.ErrorMessages.Add("Expression is empty!");
                result.IsValid = false;
            }
            else if (!IsRightCountAndOrderBracketsInExpression(expression, leftBracket, rightBracket))
            {
                result.ErrorMessages.Add("Wrong number or order of parentheses in the expression!");
                result.IsValid = false;
            }

            return result;
        }

        private static bool IsRightCountAndOrderBracketsInExpression(string expression,
            ISpecialSymbol leftBracket, ISpecialSymbol rightBracket)
        {
            // check count of brackets
            var countLeftBrackets = expression.Count(c => c == leftBracket.StringView[0]);
            var countRightBrackets = expression.Count(c => c == rightBracket.StringView[0]);
            if (countLeftBrackets != countRightBrackets)
                return false;

            // check order of brackets
            var onlyBrackets = expression
                .Where(c => c == leftBracket.StringView[0] || c == rightBracket.StringView[0])
                .Aggregate("", (res, next) => res + next);

            while (onlyBrackets.Contains(leftBracket.StringView + rightBracket.StringView))
                onlyBrackets = onlyBrackets.Replace(leftBracket.StringView + rightBracket.StringView, "");

            var result = string.IsNullOrEmpty(onlyBrackets);

            return result;
        }

    }
}
