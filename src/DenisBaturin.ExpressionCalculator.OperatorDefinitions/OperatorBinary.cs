using System;

namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class OperatorBinary : Operator
    {
        public sealed override OperatorType Type => OperatorType.Binary;
        public abstract Func<decimal, decimal, decimal> PerformAction { get; }
    }
}