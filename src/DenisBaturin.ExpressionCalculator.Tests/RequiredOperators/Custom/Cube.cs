using System;
using System.Collections.Generic;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom
{
    public class Cube : OperatorFunction
    {
        public override string StringView => "cube";

        public override Func<List<decimal>, decimal> PerformAction => args =>
        {
            if (args.Count != 1)
            {
                throw new ArgumentException($"The function {StringView}() takes only one argument!");
            }

            return args[0] * args[0] * args[0];
        };
    }
}