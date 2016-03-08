using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    /// <summary>
    ///     addition (unary prefix)
    /// </summary>
    [Export(typeof (Operator))]
    internal class AdditionUnaryPrefix : OperatorUnaryPrefix
    {
        public override string StringView => "+";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal> PerformAction =>
            arg => arg;
    }
}