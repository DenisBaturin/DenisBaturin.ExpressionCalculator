using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom
{
    public class Plus : OperatorBinary
    {
        public override string StringView => "plus";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 + arg2;
    }
}