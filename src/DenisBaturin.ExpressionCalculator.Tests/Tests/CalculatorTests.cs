using System;
using System.Globalization;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry;
using NUnit.Framework;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{
    [TestFixture]
    public class CalculatorTests
    {

        // Cases for InsertMultiplicationOperation method
        private static readonly object[] InsertMultiplicationOperationCases =
        {
            new object[] {"2(3)", 2M*3M, "Number before LeftBracket"},
            new object[] {"pi(3)", 3.14159265358979323846264338327950288M*3M, "Constant before LeftBracket"},
            new object[] {"(1+2)(3+4)", (1M + 2M)*(3M + 4M), "RightBracket before LeftBracket"},
            new object[] {"2pi", 2M*3.14159265358979323846264338327950288M, "Number before Constant"},
            new object[] {"pi pi2", 19.739208802178717237668982M, "Constant before Constant"},
            new object[] {"(1+2)pi", (1M + 2M)*3.14159265358979323846264338327950288M, "RightBracket before Constant"},
            new object[] {"2 3", 2M*3M, "Number before Number"},
            new object[] {"pi 2", 3.14159265358979323846264338327950288M*2M, "Constant before Number"},
            new object[] {"(1+2)3", (1M + 2M)*3M, "RightBracket before Number"},
            new object[] {"2sin(1)", 1.682941969615792M, "Number before Function"},
            new object[] {"pi sin(1)", 2.6435590640814545871902867732M, "Constant before Function"},
            new object[] {"(1+2)sin(1)", 2.524412954423688M, "RightBracket before Function"}
        };

        // Cases for CalculateConstant method
        private static readonly object[] CalculateConstantCases =
        {
            new object[] {"pi", 3.14159265358979323846264338327950288M},
            new object[] {"(3*pi2+2)", 20.849555921538759430775860300M},
            new object[] {"pi+3", 6.14159265358979323846264338327950288M}
        };

        // Cases for CalculateNegatiateFunctions method
        private static readonly object[] CalculateNegatiateFunctionsCases =
        {
            new object[] {"-sin(1)", -0.841470984807896M},
            new object[] {"-sin(1)-sin(1)", -1.682941969615792M}
        };

        [Test]
        [TestCase("7+3=4+6", 1)]
        [TestCase("7+3=4+7", 0)]
        [TestCase("3+8>(9-4)", 1)]
        [TestCase("2+5>(12-1)", 0)]
        [TestCase("2+2*2<4", 0)]
        [TestCase("2+2*2<10", 1)]
        [TestCase("2+2*2+4<=10", 1)]
        [TestCase("2+2*2+3<=10", 1)]
        [TestCase("2+2*2+4>=10", 1)]
        [TestCase("2+2*2+5>=10", 1)]
        public void CalculateBooleanConditions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCase("abs(-50.5)", 50.5)]
        [TestCase("rem(12,5)", 2)]
        [TestCase("sign(-5)", -1)]
        [TestCase("sqrt(81)", 9)]
        [TestCase("round(12.3456789)", 12)]
        [TestCase("pow(2,3)", 8)]
        public void CalculateBuiltInFunctions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCaseSource(nameof(CalculateConstantCases))]
        public void CalculateConstant(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            // Constant pi = 3.14159265358979323846264338327950288M
            calculator.AddOperator(new Pi());

            // Constant pi2 = 6.28318530717958647692528676655900576M
            calculator.AddOperator(new Pi2());

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        public void CalculateDecimalExpressionsInDifferentCultures()
        {
            // Russian culture uses "," as a decimal separator and ";" as a list separator
            var russianCalculator = new Calculator(new CultureInfo("ru-RU"));
            var result1 = russianCalculator.CalculateExpression("0,245 + 0,3").Answer;
            Assert.IsTrue(result1 == 0.545M);
            var result2 = russianCalculator.CalculateExpression("-0,245 + 0,3").Answer;
            Assert.IsTrue(result2 == 0.055M);
            var result3 = russianCalculator.CalculateExpression("pow(2;3)").Answer;
            Assert.IsTrue(result3 == 8M);

            // American culture uses "." as a decimal separator and "," as a list separator
            var americanCalculator = new Calculator(new CultureInfo("en-US"));
            var result11 = americanCalculator.CalculateExpression("0.245 + 0.3").Answer;
            Assert.IsTrue(result11 == 0.545M);
            var result22 = americanCalculator.CalculateExpression("-0.245 + 0.3").Answer;
            Assert.IsTrue(result22 == 0.055M);
            var result33 = americanCalculator.CalculateExpression("pow(2,3)").Answer;
            Assert.IsTrue(result33 == 8M);

            // Invariant culture uses "." as a decimal separator and "," as a list separator
            var invariantCultureCalculator = new Calculator();
            var result111 = invariantCultureCalculator.CalculateExpression("0.245 + 0.3").Answer;
            Assert.IsTrue(result111 == 0.545M);
            var result222 = invariantCultureCalculator.CalculateExpression("-0.245 + 0.3").Answer;
            Assert.IsTrue(result222 == 0.055M);
            var result333 = invariantCultureCalculator.CalculateExpression("pow(2,3)").Answer;
            Assert.IsTrue(result333 == 8M);
        }

        [Test]
        [TestCase("5+2*3*1+2((1-2)(2-3))", 13)]
        [TestCase("5+2*3*1+2((1-2)(2-3))*-1", 9)]
        [TestCase("4^2-2*3^2 +4", 2)]
        [TestCase("(3+2)(1+-2)(1--2)(1-+8)", 105)]
        [TestCase("(3+2)(1+-2)(1++2)(1-+8)", 105)]
        public void CalculateLongExpressions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCaseSource(nameof(CalculateNegatiateFunctionsCases))]
        public void CalculateNegatiateExpressions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            // sin
            calculator.AddOperator(new Sin());

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCase("-1+1", 0)]
        [TestCase("--1", 1)]
        [TestCase("(-2)", -2)]
        [TestCase("(-2)(-2)", 4)]
        [TestCase("-pow(-2,-3)", 0.125)]
        public void CalculateNegativeExpressions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        public void CalculateOperatorsInRightOrder()
        {
            var calculator = TestsUtil.GetCalculator();

            const string expression = "( 10 + 2^3 * 3 ) + 4^3  -  ( 16 : 2  - 1 ) * 5 - 150 : 5^2";
            var actual = calculator.CalculateExpression(expression).Answer;
            const decimal expected = 57M;
            Assert.AreEqual(expected, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCase("1+2", 3)]
        [TestCase("5-2", 3)]
        [TestCase("5+2", 7)]
        [TestCase("2*3", 6)]
        [TestCase("10/5", 2)]
        [TestCase("2+5-1", 6)]
        [TestCase("2+3*4", 14)]
        public void CalculateSimpleExpressions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        public void CalculateUnaryPostfixOperator()
        {
            var calculator = TestsUtil.GetCalculator();

            const string expression = "5!-4!";
            var actual = calculator.CalculateExpression(expression).Answer;
            const int expected = 96;

            Assert.AreEqual(expected, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCase("square(4)", 16)]
        [TestCase("cube(2)", 8)]
        [TestCase("7 plus 4 razdelit 2 umnogit 4 minus 10", 5)]
        public void CalculateCustomFunctions(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            calculator.AddOperator(new Square());
            calculator.AddOperator(new Cube());
            calculator.AddOperator(new Umnogit()); // x*y
            calculator.AddOperator(new Razdelit()); // x/y
            calculator.AddOperator(new Plus()); // x+y
            calculator.AddOperator(new Minus()); // x-y

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCaseSource(nameof(InsertMultiplicationOperationCases))]
        public void InsertMultiplicationOperation(string expression, decimal expected, string errorMessage)
        {
            var calculator = TestsUtil.GetCalculator();

            // Constant pi = 3.14159265358979323846264338327950288
            calculator.AddOperator(new Pi());

            // Constant pi2 = 6.28318530717958647692528676655900576
            calculator.AddOperator(new Pi2());

            // sin
            calculator.AddOperator(new Sin());

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(expected, actual, $"Expression: {expression} ({errorMessage})");
        }

        [Test]
        public void CalculationOperatorStringViewUnicode()
        {
            var calculator = TestsUtil.GetCalculator();
            
            calculator.AddOperator(new AdditionBinaryUnicode());

            const string expression = "3©©©©4";
            var actual = calculator.CalculateExpression(expression).Answer;
            const int expected = 7;

            Assert.AreEqual(expected, actual, $"Expression: {expression}");
        }

        [Test]
        public void Trigonometry()
        {
            var calculator = TestsUtil.GetCalculator();

            // cos
            calculator.AddOperator(new Cos());

            // sin
            calculator.AddOperator(new Sin());
            
            const string expression = "cos(32) + 3/sin(1)";
            var actual = calculator.CalculateExpression(expression).Answer;
            var expected = (decimal) Math.Cos(32D) + 3M/(decimal) Math.Sin(1D);
            Assert.AreEqual(expected, actual, $"Expression: {expression}");
        }

        [Test]
        [TestCase("1.2", 1.2)]
        [TestCase("1.", 1)]
        [TestCase(".2", 0.2)]
        [TestCase("1..2", 0.2)]
        [TestCase("1.1+1.2", 2.3)]
        [TestCase("1.5-1.2", 0.3)]
        [TestCase("1.8/1.2", 1.5)]
        [TestCase("1.7*1.2", 2.04)]
        [TestCase("1.7/.2", 8.5)]
        [TestCase("1.7*.2", 0.34)]
        [TestCase("1.7/2.", 0.85)]
        [TestCase("1.7*2.", 3.4)]
        public void DecimalNumbers(string expression, decimal answer)
        {
            var calculator = TestsUtil.GetCalculator();

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(answer, actual, $"Expression: {expression}");
        }

        [Test]
        public void IgnoreCaseOperatorNames()
        {
            var calculator = TestsUtil.GetCalculator();

            // Constant pi = 3.14159265358979323846264338327950288
            calculator.AddOperator(new Pi());

            const string expression = "PI pi Pi pI";
            const decimal expected = 97.40909103400243723644033269m;

            var actual = calculator.CalculateExpression(expression).Answer;
            Assert.AreEqual(expected, actual, $"Expression: {expression}");
        }

    }
}