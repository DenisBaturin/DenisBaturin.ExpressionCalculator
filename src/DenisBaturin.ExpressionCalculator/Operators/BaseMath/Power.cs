using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    /// <summary>
    ///     to the power of
    /// </summary>
    [Export(typeof (Operator))]
    internal class Power : OperatorBinary
    {
        public override string StringView => "^";
        public override PriorityLevel Priority => PriorityLevel.One;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => (decimal) Math.Pow((double) arg1, (double) arg2);
    }
}