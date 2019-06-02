using NUnit.Framework;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{

    [TestFixture]
    public class CacheTests
    {

        public class TestCase
        {
            public string Expression;
            public decimal ExpectedAnswer;

            public override string ToString()
            {
                return Expression;
            }
        }

        public static TestCase[] CacheParameterDoesNotAffectTheResultTestCases =
        {
            new TestCase {Expression = "5+2*3*1+2((1-2)(2-3))", ExpectedAnswer = 13M},
            new TestCase {Expression = "5+2*3*1+2((1-2)(2-3))*-1", ExpectedAnswer = 9M},
            new TestCase {Expression = "4^2-2*3^2 +4", ExpectedAnswer = 2M},
            new TestCase {Expression = "(3+2)(1+-2)(1--2)(1-+8)", ExpectedAnswer = 105},
            new TestCase {Expression = "(3+2)(1+-2)(1++2)(1-+8)", ExpectedAnswer = 105M},
        };

        [TestCaseSource(nameof(CacheParameterDoesNotAffectTheResultTestCases))]
        public void CacheParameterDoesNotAffectTheResultTest(TestCase testCase)
        {
            var calculator = new Calculator();

            calculator.CacheMode = false; 
            var actualAnswer = calculator.CalculateExpression(testCase.Expression).Answer;
            Assert.AreEqual(testCase.ExpectedAnswer, actualAnswer, $"Expression: {testCase.Expression}");

            calculator.CacheMode = true;
            var actualAnswerWithCacheParam = calculator.CalculateExpression(testCase.Expression).Answer;
            Assert.AreEqual(testCase.ExpectedAnswer, actualAnswerWithCacheParam, $"Expression: {testCase.Expression}");
        }
    }
}