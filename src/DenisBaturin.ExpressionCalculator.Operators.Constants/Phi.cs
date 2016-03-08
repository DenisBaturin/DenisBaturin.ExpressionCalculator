using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Constants
{
    [Export(typeof (Operator))]
    public class Phi : OperatorConstant
    {
        public override string StringView => "PHI";

        public override Func<decimal> PerformAction =>
            () => 1.61803398874989484820458683436563811M;
    }
}