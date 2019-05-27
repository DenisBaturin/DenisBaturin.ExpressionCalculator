using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Required
{
    /// <summary>
    /// Multiplication operator of two numbers. It is necessary for correction mode.
    /// </summary>
    [Export(typeof(Operator))]
    internal class Multiplication : OperatorBinary
    {
        public override string StringView => "*";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 * arg2;
    }
}