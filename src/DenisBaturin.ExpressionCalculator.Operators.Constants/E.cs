using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Constants
{
    [Export(typeof (Operator))]
    public class E : OperatorConstant
    {
        public override string StringView => "E";

        public override Func<decimal> PerformAction =>
            () => 2.71828182845904523536028747135266249M;
    }
}