namespace DenisBaturin.ExpressionCalculator.Tokens
{
    internal interface ISpecialSymbol
    {
        string StringView { get; }
        SpecialSymbolType Type { get; }
    }
}