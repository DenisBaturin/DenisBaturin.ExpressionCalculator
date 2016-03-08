using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Operators.Functions
{
    [Export(typeof (Operator))]
    internal class Log : OperatorFunction
    {
        public override string StringView => "log";

        public override Func<List<decimal>, decimal> PerformAction => args =>
            {
                if (args.Count < 1 || args.Count>2)
                {
                    throw new ArgumentException($"The function {StringView}() takes only one or two arguments!");
                }

                switch (args.Count)
                {
                    case 1:
                        return (decimal) Math.Log((double) args[0]); // base=e
                    case 2:
                        return (decimal) Math.Log((double) args[0], (double) args[1]);
                    default:
                        throw new ArgumentOutOfRangeException(args.Count.ToString());
                }
            };
    }
}