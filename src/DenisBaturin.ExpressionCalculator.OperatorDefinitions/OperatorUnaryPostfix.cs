using System;

namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class OperatorUnaryPostfix : Operator
    {
        public sealed override PriorityLevel Priority => PriorityLevel.One;
        public sealed override OperatorType Type => OperatorType.UnaryPostfix;
        public abstract Func<decimal, decimal> PerformAction { get; }
    }
}