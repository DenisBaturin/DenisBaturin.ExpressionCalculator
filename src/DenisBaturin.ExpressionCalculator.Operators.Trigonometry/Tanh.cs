using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Trigonometry
{
    [Export(typeof (Operator))]
    public class Tanh : OperatorFunction
    {
        public override string StringView => "tanh";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return (decimal) Math.Tanh((double) args[0]);
        };
    }
}