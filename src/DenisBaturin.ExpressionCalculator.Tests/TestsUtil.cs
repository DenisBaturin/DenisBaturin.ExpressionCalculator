namespace DenisBaturin.ExpressionCalculator.Tests
{
    public static class TestsUtil
    {
        /// <summary>
        ///     Factory method to get new calculator instance
        /// </summary>
        /// <returns></returns>
        public static Calculator GetCalculator()
        {
            return new Calculator
            { TraceMode = true };
        }

    }
}