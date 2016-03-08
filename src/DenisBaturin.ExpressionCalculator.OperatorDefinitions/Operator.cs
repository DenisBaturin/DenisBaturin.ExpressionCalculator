namespace DenisBaturin.ExpressionCalculator.OperatorDefinitions
{
    public abstract class Operator
    {
        public abstract string StringView { get; }
        public abstract PriorityLevel Priority { get; }
        public abstract OperatorType Type { get; }
    }
}