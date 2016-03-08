using System;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Constants
{
    [Export(typeof (Operator))]
    public class Pi : OperatorConstant
    {
        public override string StringView => "PI";

        public override Func<decimal> PerformAction =>
            () => 3.14159265358979323846264338327950288M;
    }
}