using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Functions
{
    [Export(typeof (Operator))]
    internal class Sign : OperatorFunction
    {
        public override string StringView => "sign";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return Math.Sign(args[0]);
        };
    }
}