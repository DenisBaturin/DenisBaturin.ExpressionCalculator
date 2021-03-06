using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.BaseMath
{
    [Export(typeof (Operator))]
    internal class Modulus : OperatorBinary
    {
        public override string StringView => "%";
        public override PriorityLevel Priority => PriorityLevel.One;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1%arg2;
    }
}