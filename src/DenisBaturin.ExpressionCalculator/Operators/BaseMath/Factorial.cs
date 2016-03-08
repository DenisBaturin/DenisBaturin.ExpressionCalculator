using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    /// <summary>
    ///     factorial
    /// </summary>
    [Export(typeof (Operator))]
    internal class Factorial : OperatorUnaryPostfix
    {
        public override string StringView => "!";

        public override Func<decimal, decimal> PerformAction =>
            delegate(decimal arg)
            {
                decimal result = 1;
                for (var i = arg; i > 1; i--)
                    result *= i;
                return result;
            };
    }
}