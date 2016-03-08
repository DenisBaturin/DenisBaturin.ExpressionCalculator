using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Trigonometry
{
    [Export(typeof (Operator))]
    public class Sinh : OperatorFunction
    {
        public override string StringView => "sinh";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return (decimal) Math.Sinh((double) args[0]);
        };
    }
}