using System;
using System.Globalization;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Custom;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Trigonometry;
using NUnit.Framework;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{
    [TestFixture]
    public class CalculateExpressionTests
    {
        public class TestCase
        {
            public string Description;
            public string Expression;
            public Operator[] AdditionalOperators = new Operator[0];
            public CultureInfo CultureInfo;
            public decimal ExpectedAnswer;

            public override string ToString()
            {
                return Expression;
            }
        }

        private static void TestBody(TestCase testCase)
        {
            TestContext.WriteLine(testCase.Description);

            var calculator = new Calculator(testCase.CultureInfo);

            foreach (var additionalOperator in testCase.AdditionalOperators)
            {
                calculator.AddOperator(additionalOperator);
            }

            var actualAnswer = calculator.CalculateExpression(testCase.Expression).Answer;

            Assert.AreEqual(testCase.ExpectedAnswer, actualAnswer, $"Expression: {testCase.Expression}");
        }

        public static TestCase[] CalculateBooleanConditionsTestCases =
        {
            new TestCase {Expression = "7+3=4+6", ExpectedAnswer = 1M},
            new TestCase {Expression = "7+3=4+7", ExpectedAnswer = 0M},
            new TestCase {Expression = "3+8>(9-4)", ExpectedAnswer = 1M},
            new TestCase {Expression = "2+5>(12-1)", ExpectedAnswer = 0M},
            new TestCase {Expression = "2+2*2<4", ExpectedAnswer = 0M},
            new TestCase {Expression = "2+2*2<10", ExpectedAnswer = 1M},
            new TestCase {Expression = "2+2*2+4<=10", ExpectedAnswer = 1M},
            new TestCase {Expression = "2+2*2+3<=10", ExpectedAnswer = 1M},
            new TestCase {Expression = "2+2*2+4>=10", ExpectedAnswer = 1M},
            new TestCase {Expression = "2+2*2+5>=10", ExpectedAnswer = 1M},
        };

        [TestCaseSource(nameof(CalculateBooleanConditionsTestCases))]
        public void CalculateTests(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateBuiltInFunctionsTestCases =
        {
            new TestCase {Expression = "abs(-50.5)", ExpectedAnswer = 50.5M},
            new TestCase {Expression = "rem(12,5)", ExpectedAnswer = 2M},
            new TestCase {Expression = "sign(-5)", ExpectedAnswer = -1M},
            new TestCase {Expression = "sqrt(81)", ExpectedAnswer = 9M},
            new TestCase {Expression = "round(12.3456789)", ExpectedAnswer = 12M},
            new TestCase {Expression = "pow(2,3)", ExpectedAnswer = 8M},
        };

        [TestCaseSource(nameof(CalculateBuiltInFunctionsTestCases))]
        public void CalculateBuiltInFunctions(TestCase testCase)
        {
            TestBody(testCase);
        }

        private static readonly TestCase[] CalculateConstantCases =
        {
            new TestCase
            {
                Expression = "pi",
                AdditionalOperators = new Operator[] {new Pi(), new Pi2()},
                ExpectedAnswer = 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Expression = "(3*pi2+2)",
                AdditionalOperators = new Operator[] {new Pi(), new Pi2()},
                ExpectedAnswer = 20.849555921538759430775860300M
            },
            new TestCase
            {
                Expression = "pi+3",
                AdditionalOperators = new Operator[] {new Pi(), new Pi2()},
                ExpectedAnswer = 6.14159265358979323846264338327950288M
            },
        };

        [TestCaseSource(nameof(CalculateConstantCases))]
        public void CalculateConstant(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateLongExpressionsTestCases =
        {
            new TestCase {Expression = "5+2*3*1+2((1-2)(2-3))", ExpectedAnswer = 13M},
            new TestCase {Expression = "5+2*3*1+2((1-2)(2-3))*-1", ExpectedAnswer = 9M},
            new TestCase {Expression = "4^2-2*3^2 +4", ExpectedAnswer = 2M},
            new TestCase {Expression = "(3+2)(1+-2)(1--2)(1-+8)", ExpectedAnswer = 105},
            new TestCase {Expression = "(3+2)(1+-2)(1++2)(1-+8)", ExpectedAnswer = 105M},
        };

        [TestCaseSource(nameof(CalculateLongExpressionsTestCases))]
        public void CalculateLongExpressions(TestCase testCase)
        {
            TestBody(testCase);
        }

        private static readonly TestCase[] CalculateNegativeFunctionsTestCases =
        {
            new TestCase
            {
                Expression = "-sin(1)",
                AdditionalOperators = new Operator[] {new Sin()},
                ExpectedAnswer = -0.841470984807896M
            },
            new TestCase
            {
                Expression = "-sin(1)-sin(1)",
                AdditionalOperators = new Operator[] {new Sin()},
                ExpectedAnswer = -1.682941969615792M
            },
        };

        [TestCaseSource(nameof(CalculateNegativeFunctionsTestCases))]
        public void CalculateNegativeFunctions(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateNegativeExpressionsTestCases =
        {
            new TestCase {Expression = "-1+1", ExpectedAnswer = 0M},
            new TestCase {Expression = "--1", ExpectedAnswer = 1M},
            new TestCase {Expression = "(-2)", ExpectedAnswer = -2M},
            new TestCase {Expression = "(-2)(-2)", ExpectedAnswer = 4M},
            new TestCase {Expression = "-pow(-2,-3)", ExpectedAnswer = 0.125M},
        };

        [TestCaseSource(nameof(CalculateNegativeExpressionsTestCases))]
        public void CalculateNegativeExpressions(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateSimpleExpressionsTestCases =
        {
            new TestCase {Expression = "1+2", ExpectedAnswer = 3M},
            new TestCase {Expression = "5-2", ExpectedAnswer = 3M},
            new TestCase {Expression = "5+2", ExpectedAnswer = 7M},
            new TestCase {Expression = "2*3", ExpectedAnswer = 6M},
            new TestCase {Expression = "10/5", ExpectedAnswer = 2M},
            new TestCase {Expression = "2+5-1", ExpectedAnswer = 6M},
            new TestCase {Expression = "2+3*4", ExpectedAnswer = 14M},
        };

        [TestCaseSource(nameof(CalculateSimpleExpressionsTestCases))]
        public void CalculateSimpleExpressions(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateCustomFunctionsTestCases =
        {
            new TestCase
            {
                Expression = "square(4)",
                AdditionalOperators = new Operator[] {new Square(),},
                ExpectedAnswer = 16M
            },
            new TestCase
            {
                Expression = "cube(2)",
                AdditionalOperators = new Operator[] {new Cube(),},
                ExpectedAnswer = 8M
            },
            new TestCase
            {
                Expression = "7 plus 4 razdelit 2 umnogit 4 minus 10",
                AdditionalOperators = new Operator[] {new Plus(), new Razdelit(), new Umnogit(), new Minus(),},
                ExpectedAnswer = 5M
            },
        };

        [TestCaseSource(nameof(CalculateCustomFunctionsTestCases))]
        public void CalculateCustomFunctions(TestCase testCase)
        {
            TestBody(testCase);
        }

        private static readonly TestCase[] InsertMultiplicationOperationTestCases =
        {
            new TestCase
            {
                Description = "Number before LeftBracket",
                Expression = "2(3)", ExpectedAnswer = 2M * 3M
            },
            new TestCase
            {
                Description = "Constant before LeftBracket",
                AdditionalOperators = new Operator[] {new Pi()},
                Expression = "pi(3)", ExpectedAnswer = 3.14159265358979323846264338327950288M * 3M
            },
            new TestCase
            {
                Description = "RightBracket before LeftBracket",
                Expression = "(1+2)(3+4)", ExpectedAnswer = (1M + 2M) * (3M + 4M)
            },

            new TestCase
            {
                Description = "Number before Constant",
                AdditionalOperators = new Operator[] {new Pi()},
                Expression = "2pi", ExpectedAnswer = 2M * 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Description = "Constant before Constant",
                AdditionalOperators = new Operator[] {new Pi(), new Pi2(),},
                Expression = "pi pi2", ExpectedAnswer = 19.739208802178717237668982M
            },
            new TestCase
            {
                Description = "RightBracket before Constant",
                AdditionalOperators = new Operator[] {new Pi()},
                Expression = "(1+2)pi", ExpectedAnswer = (1M + 2M) * 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Description = "Number before Number",
                Expression = "2 3", ExpectedAnswer = 2M * 3M
            },
            new TestCase
            {
                Description = "Constant before Number",
                AdditionalOperators = new Operator[] {new Pi()},
                Expression = "pi 2", ExpectedAnswer = 3.14159265358979323846264338327950288M * 2M
            },
            new TestCase
            {
                Description = "RightBracket before Number",
                Expression = "(1+2)3", ExpectedAnswer = (1M + 2M) * 3M
            },
            new TestCase
            {
                Description = "Number before Function",
                AdditionalOperators = new Operator[] {new Sin()},
                Expression = "2sin(1)", ExpectedAnswer = 1.682941969615792M
            },
            new TestCase
            {
                Description = "Constant before Function",
                AdditionalOperators = new Operator[] {new Pi(), new Sin(),},
                Expression = "pi sin(1)", ExpectedAnswer = 2.6435590640814545871902867732M
            },
            new TestCase
            {
                Description = "RightBracket before Function",
                AdditionalOperators = new Operator[] {new Sin()},
                Expression = "(1+2)sin(1)", ExpectedAnswer = 2.524412954423688M
            },
        };

        [TestCaseSource(nameof(InsertMultiplicationOperationTestCases))]
        public void InsertMultiplicationOperation(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] DecimalNumbersTestCases =
        {
            new TestCase {Expression = "1.2", ExpectedAnswer = 1.2M},
            new TestCase {Expression = "1.", ExpectedAnswer = 1M},
            new TestCase {Expression = ".2", ExpectedAnswer = 0.2M},
            new TestCase {Expression = "1..2", ExpectedAnswer = 0.2M},
            new TestCase {Expression = "1.1+1.2", ExpectedAnswer = 2.3M},
            new TestCase {Expression = "1.5-1.2", ExpectedAnswer = 0.3M},
            new TestCase {Expression = "1.8/1.2", ExpectedAnswer = 1.5M},
            new TestCase {Expression = "1.7*1.2", ExpectedAnswer = 2.04M},
            new TestCase {Expression = "1.7/.2", ExpectedAnswer = 8.5M},
            new TestCase {Expression = "1.7*.2", ExpectedAnswer = 0.34M},
            new TestCase {Expression = "1.7/2.", ExpectedAnswer = 0.85M},
            new TestCase {Expression = "1.7*2.", ExpectedAnswer = 3.4M},
        };

        [TestCaseSource(nameof(DecimalNumbersTestCases))]
        public void DecimalNumbers(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] IgnoreCaseOperatorNamesTestCases =
        {
            new TestCase
            {
                Expression = "PI",
                AdditionalOperators = new Operator[] {new Pi()},
                ExpectedAnswer = 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Expression = "pi",
                AdditionalOperators = new Operator[] {new Pi()},
                ExpectedAnswer = 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Expression = "Pi",
                AdditionalOperators = new Operator[] {new Pi()},
                ExpectedAnswer = 3.14159265358979323846264338327950288M
            },
            new TestCase
            {
                Expression = "pI",
                AdditionalOperators = new Operator[] {new Pi()},
                ExpectedAnswer = 3.14159265358979323846264338327950288M
            },
        };

        [TestCaseSource(nameof(IgnoreCaseOperatorNamesTestCases))]
        public void IgnoreCaseOperatorNames(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateDecimalExpressionsInDifferentCulturesTestCases =
        {
            // Russian culture uses "," as a decimal separator and ";" as a list separator
            new TestCase
            {
                Expression = "0,245 + 0,3",
                CultureInfo = new CultureInfo("ru-RU"),
                ExpectedAnswer = 0.545M
            },
            new TestCase
            {
                Expression = "-0,245 + 0,3",
                CultureInfo = new CultureInfo("ru-RU"),
                ExpectedAnswer = 0.055M
            },
            new TestCase
            {
                Expression = "pow(2;3)",
                CultureInfo = new CultureInfo("ru-RU"),
                ExpectedAnswer = 8M
            },
            // American culture uses "." as a decimal separator and "," as a list separator
            new TestCase
            {
                Expression = "0.245 + 0.3",
                CultureInfo = new CultureInfo("en-US"),
                ExpectedAnswer = 0.545M
            },
            new TestCase
            {
                Expression = "-0.245 + 0.3",
                CultureInfo = new CultureInfo("en-US"),
                ExpectedAnswer = 0.055M
            },
            new TestCase
            {
                Expression = "pow(2,3)",
                CultureInfo = new CultureInfo("en-US"),
                ExpectedAnswer = 8M
            },
            // Invariant culture uses "." as a decimal separator and "," as a list separator
            new TestCase
            {
                Expression = "0.245 + 0.3",
                ExpectedAnswer = 0.545M
            },
            new TestCase
            {
                Expression = "-0.245 + 0.3",
                ExpectedAnswer = 0.055M
            },
            new TestCase
            {
                Expression = "pow(2,3)",
                ExpectedAnswer = 8M
            },
        };

        [TestCaseSource(nameof(CalculateDecimalExpressionsInDifferentCulturesTestCases))]
        public void CalculateDecimalExpressionsInDifferentCultures(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateOperatorsInRightOrderTestCases =
        {
            new TestCase
            {
                Expression = "( 10 + 2^3 * 3 ) + 4^3  -  ( 16 : 2  - 1 ) * 5 - 150 : 5^2",
                ExpectedAnswer = 57M
            },
        };

        [TestCaseSource(nameof(CalculateOperatorsInRightOrderTestCases))]
        public void CalculateOperatorsInRightOrder(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculateUnaryPostfixOperatorTestCases =
        {
            new TestCase
            {
                Expression = "5!-4!",
                ExpectedAnswer = 96M
            },
        };

        [TestCaseSource(nameof(CalculateUnaryPostfixOperatorTestCases))]
        public void CalculateUnaryPostfixOperator(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] CalculationOperatorStringViewUnicodeTestCases =
        {
            new TestCase
            {
                Expression = "3©©©©4",
                AdditionalOperators = new Operator[] {new AdditionBinaryUnicode(),},
                ExpectedAnswer = 7M
            },
        };

        [TestCaseSource(nameof(CalculationOperatorStringViewUnicodeTestCases))]
        public void CalculationOperatorStringViewUnicode(TestCase testCase)
        {
            TestBody(testCase);
        }

        public static TestCase[] TrigonometryTestCases =
        {
            new TestCase
            {
                Expression = "cos(32) + 3/sin(1)",
                AdditionalOperators = new Operator[] {new Cos(), new Sin(),},
                ExpectedAnswer = (decimal) Math.Cos(32D) + 3M / (decimal) Math.Sin(1D)
            },
        };

        [TestCaseSource(nameof(TrigonometryTestCases))]
        public void Trigonometry(TestCase testCase)
        {
            TestBody(testCase);
        }
    }
}