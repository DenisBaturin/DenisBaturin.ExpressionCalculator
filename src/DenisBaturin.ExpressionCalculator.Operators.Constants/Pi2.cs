using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Constants
{
    [Export(typeof (Operator))]
    public class Pi2 : OperatorConstant
    {
        public override string StringView => "PI2";

        public override Func<decimal> PerformAction =>
            () => 6.28318530717958647692528676655900576M;
    }
}