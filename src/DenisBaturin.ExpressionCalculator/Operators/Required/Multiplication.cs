using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

//using System.ComponentModel.Composition;

namespace DenisBaturin.ExpressionCalculator.Operators.Required
{
    //[Export(typeof (Operator))]
    internal class Multiplication : OperatorBinary
    {
        public override string StringView => "*";
        public override PriorityLevel Priority => PriorityLevel.Two;

        public override Func<decimal, decimal, decimal> PerformAction =>
            (arg1, arg2) => arg1*arg2;
    }
}