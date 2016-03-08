namespace DenisBaturin.ExpressionCalculator.Tokens
{
    internal class SpecialSymbol : ISpecialSymbol
    {
        public SpecialSymbol(string stringView, SpecialSymbolType type)
        {
            StringView = stringView;
            Type = type;
        }

        public string StringView { get; }
        public SpecialSymbolType Type { get; }
    }
}