using System;

namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class OperatorConstant : Operator
    {
        public sealed override PriorityLevel Priority => PriorityLevel.Zero;
        public sealed override OperatorType Type => OperatorType.Constant;
        public abstract Func<decimal> PerformAction { get; }
    }
}