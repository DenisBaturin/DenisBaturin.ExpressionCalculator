using System;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry;
using NUnit.Framework;
using static DenisBaturin.ExpressionCalculator.Tests.TestsUtil;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{

    [TestFixture]
    public class ThrowExceptionTests
    {

        [Test]
        public void AddOperatorWithWhitespaceAtStringView_ThrowArgumentException()
        {
            var calculator = GetCalculator();

            var exception = Assert.Catch<ArgumentException>(()
                => calculator.AddOperator(new ConstantWithWhitespaceAtStringView()));
            Console.WriteLine(exception.Message);
        }

        [Test]
        public void AddSameOperators_ThrowArgumentException()
        {
            var calculator = GetCalculator();

            var @operator1 = new Pi();
            var @operator2 = new Pi();

            calculator.AddOperator(@operator1);

            var exception = Assert.Catch<ArgumentException>(() => calculator.AddOperator(@operator2));
            Console.WriteLine(exception.Message);
        }

        [Test]
        public void AddSameOperatorsStringViewDifferentRegister_ThrowArgumentException()
        {
            var calculator = GetCalculator();

            // Constant "pi" (StringView in lower case)
            var lowerCase = new Pi();
            // Constant "PI" (StringView in upper case)
            var upperCase = new PiUpper();

            calculator.AddOperator(lowerCase);

            var exception = Assert.Catch<ArgumentException>(() => calculator.AddOperator(upperCase));
            Console.WriteLine(exception.Message);
        }

        [Test]
        [TestCase("(2)(5)")]
        [TestCase("2 5")]
        [TestCase("2pi")]
        [TestCase("2sin(1)")]
        public void WithoutCorrection_ThrowFormatException(string expression)
        {
            var calculator = GetCalculator();
            calculator.CorrectionMode = false;

            // Constant pi = 3.14159265358979323846264338327950288
            calculator.AddOperator(new Pi());
            // sin
            calculator.AddOperator(new Sin());

            // ReSharper disable once JoinDeclarationAndInitializer
            Exception exception;

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression(expression));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));
        }

        [Test]
        public void InvalidExpression_TrowConcreteException()
        {
            var calculator = GetCalculator();

            // ToDo: to process all exceptions at Calculator.cs

            // ReSharper disable once JoinDeclarationAndInitializer
            Exception exception;

            calculator.AddOperator(new Sin());

            exception = Assert.Catch<ApplicationException>(() => calculator.CalculateExpression("?-2"));
            Assert.IsTrue(exception.Message.Contains("Unknown token type:"));
            Console.WriteLine(exception.Message);

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("(-1"));
            Assert.IsTrue(exception.Message.Contains("Wrong number or order of parentheses in the expression!"));
            Console.WriteLine(exception.Message);

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression(""));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Expression is empty!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("."));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression(","));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("()"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("sin-1"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Invalid operators order!"));

            exception = Assert.Catch<ArgumentException>(() => calculator.CalculateExpression("sin()"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("The function sin() takes only one argument!"));

            exception = Assert.Catch<ArgumentException>(() => calculator.CalculateExpression("sin(,)"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("The function sin() takes only one argument!"));

            exception = Assert.Catch<ArgumentException>(() => calculator.CalculateExpression("sin(1,2,3)"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("The function sin() takes only one argument!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("1**2"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("(1,2)"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));

            exception = Assert.Catch<FormatException>(() => calculator.CalculateExpression("pow((2,3),(2,3))"));
            Console.WriteLine(exception.Message);
            Assert.IsTrue(exception.Message.Contains("Wrong expression!"));
        }

    }
}