using System;
using System.Collections.Generic;

namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class OperatorFunction : Operator
    {
        public sealed override PriorityLevel Priority => PriorityLevel.One;
        public sealed override OperatorType Type => OperatorType.Function;
        public abstract Func<List<decimal>, decimal> PerformAction { get; }
    }
}