using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    /// <summary>
    ///     addition (binary)
    /// </summary>
    [Export(typeof (Operator))]
    internal class AdditionBinary : OperatorBinary
    {
        public override string StringView => "+";
        public override PriorityLevel Priority => PriorityLevel.Three;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 + arg2;
    }
}