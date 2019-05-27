using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry;
using NUnit.Framework;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{

    [TestFixture]
    public class ThrowExceptionTests
    {
        public class TestCase
        {
            public string Description;
            public string Expression;
            public Operator[] AdditionalOperators = new Operator[0];
            public string ErrorMessagePart;

            public override string ToString()
            {
                return Expression;
            }
        }

        public static TestCase[] WithoutCorrectionThrowFormatExceptionTestCases =
        {
            new TestCase
            {
                Expression = "(2)(5)"
            },
            new TestCase
            {
                Expression = "2 5"
            },
            new TestCase
            {
                Expression = "2pi",
                AdditionalOperators = new Operator[] {new Pi()},
            },
            new TestCase
            {
                Expression = "2sin(1)",
                AdditionalOperators = new Operator[] {new Sin()},
            },
        };

        [TestCaseSource(nameof(WithoutCorrectionThrowFormatExceptionTestCases))]
        public void WithoutCorrectionThrowFormatException(TestCase testCase)
        {
            TestContext.WriteLine(testCase.Description);

            var calculator = new Calculator {CorrectionMode = false};

            foreach (var additionalOperator in testCase.AdditionalOperators)
            {
                calculator.AddOperator(additionalOperator);
            }

            Assert.Throws(
                Is.TypeOf<FormatException>()
                    .And.Message.Contains("Wrong expression!"),
                () => calculator.CalculateExpression(testCase.Expression));
        }
        
        public static TestCase[] InvalidExpressionThrowApplicationExceptionTestCases =
        {
            new TestCase
            {
                Expression = "?-2",
                ErrorMessagePart = "Unknown token type:",
            },
            new TestCase
            {
                Expression = "?2",
                ErrorMessagePart = "Unknown token type:",
            },
            new TestCase
            {
                Expression = "2?",
                ErrorMessagePart = "Unknown token type:",
            },
        };

        [TestCaseSource(nameof(InvalidExpressionThrowApplicationExceptionTestCases))]
        public void InvalidExpressionThrowApplicationException(TestCase testCase)
        {
            TestContext.WriteLine(testCase.Description);

            var calculator = new Calculator();

            foreach (var additionalOperator in testCase.AdditionalOperators)
            {
                calculator.AddOperator(additionalOperator);
            }

            Assert.Throws(
                Is.TypeOf<ApplicationException>()
                    .And.Message.Contains(testCase.ErrorMessagePart),
                () => calculator.CalculateExpression(testCase.Expression));
        }

        public static TestCase[] InvalidExpressionThrowFormatExceptionTestCases =
        {
            new TestCase
            {
                Expression = "(-1",
                ErrorMessagePart = "Wrong number or order of parentheses in the expression!"
            },
            new TestCase
            {
                Expression = "",
                ErrorMessagePart = "Expression is empty!"
            },
            new TestCase
            {
                Expression = ".",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = ",",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = "()",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = "1**2",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = "(1,2)",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = "pow((2,3),(2,3))",
                ErrorMessagePart = "Wrong expression!"
            },
            new TestCase
            {
                Expression = "sin-1",
                AdditionalOperators = new Operator[] {new Sin()},
                ErrorMessagePart = "Invalid operators order!"
            },
        };

        [TestCaseSource(nameof(InvalidExpressionThrowFormatExceptionTestCases))]
        public void InvalidExpressionThrowFormatException(TestCase testCase)
        {
            TestContext.WriteLine(testCase.Description);

            var calculator = new Calculator();

            foreach (var additionalOperator in testCase.AdditionalOperators)
            {
                calculator.AddOperator(additionalOperator);
            }

            Assert.Throws(
                Is.TypeOf<FormatException>()
                    .And.Message.Contains(testCase.ErrorMessagePart),
                () => calculator.CalculateExpression(testCase.Expression));
        }
        
        public static TestCase[] InvalidExpressionThrowArgumentExceptionTestCases =
        {
            new TestCase
            {
                Expression = "sin()",
                AdditionalOperators = new Operator[] {new Sin()},
                ErrorMessagePart = "The function sin() takes only one argument!"
            },
            new TestCase
            {
                Expression = "sin(,)",
                AdditionalOperators = new Operator[] {new Sin()},
                ErrorMessagePart = "The function sin() takes only one argument!"
            },
            new TestCase
            {
                Expression = "sin(1,2,3)",
                AdditionalOperators = new Operator[] {new Sin()},
                ErrorMessagePart = "The function sin() takes only one argument!"
            },
        };

        [TestCaseSource(nameof(InvalidExpressionThrowArgumentExceptionTestCases))]
        public void InvalidExpressionThrowArgumentException(TestCase testCase)
        {
            TestContext.WriteLine(testCase.Description);

            var calculator = new Calculator();

            foreach (var additionalOperator in testCase.AdditionalOperators)
            {
                calculator.AddOperator(additionalOperator);
            }

            Assert.Throws(
                Is.TypeOf<ArgumentException>()
                    .And.Message.Contains(testCase.ErrorMessagePart),
                () => calculator.CalculateExpression(testCase.Expression));
        }
    }
}