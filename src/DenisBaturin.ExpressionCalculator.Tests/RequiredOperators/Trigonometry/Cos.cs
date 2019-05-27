using System;
using System.Collections.Generic;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry
{
    public class Cos : OperatorFunction
    {
        public override string StringView => "cos";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }

            return (decimal) Math.Cos((double) args[0]);
        };
    }
}