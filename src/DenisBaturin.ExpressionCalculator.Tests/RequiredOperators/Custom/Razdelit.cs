using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom
{
    public class Razdelit : OperatorBinary
    {
        public override string StringView => "razdelit";
        public override PriorityLevel Priority => PriorityLevel.One;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 / arg2;
    }
}