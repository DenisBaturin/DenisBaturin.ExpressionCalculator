using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    [Export(typeof (Operator))]
    internal class Negation : OperatorUnaryPrefix
    {
        public override string StringView => "-";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal> PerformAction =>
            arg => -arg;
    }
}