using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Functions
{
    [Export(typeof (Operator))]
    internal class Rem : OperatorFunction
    {
        public override string StringView => "rem";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 2)
            {
                throw new ArgumentException($"The function {StringView}() takes only two arguments!");
            }
            return (decimal) Math.IEEERemainder((double) args[0], (double) args[1]);
        };
    }
}