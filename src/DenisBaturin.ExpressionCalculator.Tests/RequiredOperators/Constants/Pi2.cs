using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants
{
    public class Pi2 : OperatorConstant
    {
        public override string StringView => "pi2";
        public override Func<decimal> PerformAction => () => 6.28318530717958647692528676655900576M;
    }
}