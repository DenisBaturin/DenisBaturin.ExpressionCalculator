using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants
{
    public class ConstantWithWhitespaceAtStringView : OperatorConstant
    {
        public override string StringView => "Constant With Whitespace At StringView";
        public override Func<decimal> PerformAction => () => 3.14159265358979323846264338327950288M;
    }
}