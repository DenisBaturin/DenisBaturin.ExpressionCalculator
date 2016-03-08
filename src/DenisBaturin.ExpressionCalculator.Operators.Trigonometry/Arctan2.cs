using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Trigonometry
{
    [Export(typeof (Operator))]
    public class Arctan2 : OperatorFunction
    {
        public override string StringView => "arctan2";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 2)
            {
                throw new ArgumentException($"The function {StringView}() takes only two arguments!");
            }
            return (decimal) Math.Atan2((double) args[0], (double) args[1]);
        };
    }
}