using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Trigonometry
{
    [Export(typeof (Operator))]
    public class Cosh : OperatorFunction
    {
        public override string StringView => "cosh";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return (decimal) Math.Cosh((double) args[0]);
        };
    }
}