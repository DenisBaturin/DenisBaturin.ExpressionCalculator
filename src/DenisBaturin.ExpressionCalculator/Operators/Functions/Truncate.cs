using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Functions
{
    [Export(typeof (Operator))]
    internal class Truncate : OperatorFunction
    {
        public override string StringView => "truncate";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return (decimal) (args[0] < 0.0M ? -Math.Floor(-(double) args[0]) : Math.Floor((double) args[0]));
        };
    }
}