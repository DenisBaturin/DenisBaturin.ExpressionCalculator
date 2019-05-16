namespace DenisBaturin.ExpressionCalculator.Tokens
{
    public interface ISpecialSymbol
    {
        string StringView { get; }
        SpecialSymbolType Type { get; }
    }
}