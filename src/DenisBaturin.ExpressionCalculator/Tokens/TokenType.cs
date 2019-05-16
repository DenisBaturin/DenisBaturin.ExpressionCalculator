namespace DenisBaturin.ExpressionCalculator.Tokens
{
    public enum TokenType
    {
        LeftBracket,
        RightBracket,
        ListSeparator,
        Number,
        OperatorConstant,
        OperatorBinary,
        OperatorUnaryPrefix,
        OperatorUnaryPostfix,
        OperatorFunction,
        ParamsList
    }
}