using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    [Export(typeof (Operator))]
    internal class Division1 : OperatorBinary
    {
        public override string StringView => ":";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1/arg2;
    }
}