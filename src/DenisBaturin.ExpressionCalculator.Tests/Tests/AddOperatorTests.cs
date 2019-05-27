using System;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;
using DenisBaturin.ExpressionCalculator.Tests.RequiredOperators.Constants;
using NUnit.Framework;

namespace DenisBaturin.ExpressionCalculator.Tests.Tests
{

    [TestFixture]
    public class AddOperatorTests
    {

        public class TestCase
        {
            public string Description;
            public Operator[] AdditionalOperators = new Operator[0];
            public Operator OperatorToAdd;
            public string ErrorMessagePart;

            public override string ToString()
            {
                return Description;
            }
        }

        public static TestCase[] AddOperatorThrowArgumentExceptionTestCases =
        {
            new TestCase
            {
                Description = "Add Operator with Whitespace at StringView should throw ArgumentException",
                OperatorToAdd = new ConstantWithWhitespaceAtStringView(),
                ErrorMessagePart = "has whitespace in the StringView"
            },
            new TestCase
            {
                Description = "Add same Operators should throw ArgumentException",
                AdditionalOperators = new Operator[] {new Pi()},
                OperatorToAdd = new Pi(),
                ErrorMessagePart = "has already declared"
            },
            new TestCase
            {
                Description =
                    "Add Operators with same StringView but different register should throw ArgumentException",
                AdditionalOperators = new Operator[] {new Pi()},
                OperatorToAdd = new PiUpper(),
                ErrorMessagePart = "has already declared"
            },
        };

        [TestCaseSource(nameof(AddOperatorThrowArgumentExceptionTestCases))]
        public void AddOperator_ThrowArgumentException(TestCase testCase)
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
                () => calculator.AddOperator(testCase.OperatorToAdd));
        }
    }
}