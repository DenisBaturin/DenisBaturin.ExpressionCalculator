using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom
{
    public class Minus : OperatorBinary
    {
        public override string StringView => "minus";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 - arg2;
    }
}