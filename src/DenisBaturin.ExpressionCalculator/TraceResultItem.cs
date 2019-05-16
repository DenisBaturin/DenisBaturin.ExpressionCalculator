using System.Collections.Generic;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator
{
    public class TraceResultItem
    {
        public string Text { get; set; }
        public List<Token> Tokens { get; set; }
    }
}