namespace DenisBaturin.ExpressionCalculator.Tokens
{
    internal enum TokenType
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