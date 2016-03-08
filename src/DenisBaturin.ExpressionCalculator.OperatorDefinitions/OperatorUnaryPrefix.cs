using System;

namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class OperatorUnaryPrefix : Operator
    {
        public sealed override OperatorType Type => OperatorType.UnaryPrefix;
        public abstract Func<decimal, decimal> PerformAction { get; }
    }
}