using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Constants
{
    [Export(typeof (Operator))]
    public class Zero : OperatorConstant
    {
        public override string StringView => "ZERO";

        public override Func<decimal> PerformAction =>
            () => 0M;
    }
}