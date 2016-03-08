using System;
using System.Collections.Generic;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry
{
    public class Sin : OperatorFunction
    {
        public override string StringView => "sin";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }
            return (decimal) Math.Sin((double) args[0]);
        };
    }
}