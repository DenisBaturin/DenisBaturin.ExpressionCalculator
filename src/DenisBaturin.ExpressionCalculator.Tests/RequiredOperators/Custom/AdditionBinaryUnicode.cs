using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom
{
    public class AdditionBinaryUnicode : OperatorBinary
    {
        public override string StringView => "©©©©";
        public override PriorityLevel Priority => PriorityLevel.Three;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1 + arg2;
    }
}