using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Boolean
{
    [Export(typeof (Operator))]
    internal class GreaterThan : OperatorBinary
    {
        public override string StringView => ">";
        public override PriorityLevel Priority => PriorityLevel.Four;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 > arg2 ? 1 : 0;
    }
}