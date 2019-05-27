using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;
using DenisBaturin.ExpressionCalculator.Operators;
using DenisBaturin.ExpressionCalculator.Operators.Required;
using DenisBaturin.ExpressionCalculator.Parsers;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator
{
    public class Calculator
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [ImportMany(typeof (Operator))] private List<Operator> _importOperators = new List<Operator>();
        private readonly OperatorsUniqueList _operators = new OperatorsUniqueList();
        private readonly SpecialSymbolsUniqueList _specialSymbols;
        private ResultInfo _resultInfo;
        public readonly CultureInfo CalculatorCultureInfo;

        public bool TraceMode { get; set; } = false;
        public bool CorrectionMode { get; set; } = true;
        
        public Calculator(CultureInfo cultureInfo = null)
        {
            CalculatorCultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            _specialSymbols = new SpecialSymbolsUniqueList
            {
                new SpecialSymbol("(", SpecialSymbolType.LeftBracket),
                new SpecialSymbol(")", SpecialSymbolType.RightBracket),
                new SpecialSymbol(CalculatorCultureInfo.TextInfo.ListSeparator, SpecialSymbolType.ListSeparator)
            };

            LoadOperators();
        }

        private void LoadOperators()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Calculator class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (Calculator).Assembly));
            // Adds "Operators" catalog if exists
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (location != null)
            {
                var operatorsDirectory = Path.Combine(location, "Operators");
                if (Directory.Exists(operatorsDirectory))
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(operatorsDirectory));
                }
            }
            //Create the CompositionContainer with the parts in the catalog
            var compositionContainer = new CompositionContainer(catalog);
            //Fill the imports of this object
            compositionContainer.ComposeParts(this);

            foreach (var @operator in _importOperators)
            {
                AddOperator(@operator);
            }
        }

        public void AddOperator(Operator @operator)
        {
            _operators.Add(@operator);
        }

        public IEnumerable<string> GetOperatorsList()
        {
            var operators = _operators
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.Type)
                .ThenBy(x => x.StringView)
                .Select(@operator => @operator.StringView.PadRight(13) + @operator.Type);
            return operators;
        }

        /// <summary>
        ///     Main method for calculation.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ResultInfo CalculateExpression(string expression)
        {
            _resultInfo = new ResultInfo();

            var expressionValidateResult = StringExpressionValidator.Validate(expression,
                _specialSymbols.Single(ss => ss.Type == SpecialSymbolType.LeftBracket),
                _specialSymbols.Single(ss => ss.Type == SpecialSymbolType.RightBracket));
            if (!expressionValidateResult.IsValid)
            {
                var errors = expressionValidateResult.ErrorMessages.Aggregate("",
                    (current, errorMessage) => current + errorMessage + Environment.NewLine);
                throw new FormatException(errors);
            }

            var parser = new ExpressionParser(CalculatorCultureInfo, _specialSymbols, _operators);
            var tokens = parser.ConvertExpressionToTokens(expression);

            _resultInfo.TraceResult.Add(new TraceResultItem
            {
                Text = "step: tokenization",
                Tokens = new List<Token>(tokens)
            });

            tokens = CorrectionOfTokens(tokens);
            tokens = ProcessingBrackets(tokens);

            // after correction and processing all brackets process final expression
            var result = CalculateResultFromTokensWithoutBrackets(tokens);

            _resultInfo.Answer = result;

            return _resultInfo;
        }

        /// <summary>
        ///     Insert multiplication operator between tokens if nedeed.
        ///     For example: (1+2)(3+4) => (1+2)*(3+4), 2(3+4) => 2*(3+4)
        /// </summary>
        /// <param name="tokens"></param>
        private List<Token> CorrectionOfTokens(List<Token> tokens)
        {
            if (!CorrectionMode) return tokens;

            for (var position = 1; position < tokens.Count; position++)
            {
                var currentToken = tokens[position];
                var previousToken = tokens[position - 1];

                if ((previousToken.Type == TokenType.Number
                     || previousToken.Type == TokenType.OperatorConstant
                     || previousToken.Type == TokenType.RightBracket)
                    &&
                    (currentToken.Type == TokenType.Number
                     || currentToken.Type == TokenType.OperatorConstant
                     || currentToken.Type == TokenType.LeftBracket
                     || currentToken.Type == TokenType.OperatorFunction))
                {
                    tokens.Insert(position, new Token(new Multiplication(), CalculatorCultureInfo));
                }
            }

            _resultInfo.TraceResult.Add(new TraceResultItem
            {
                Text = "step: correction",
                Tokens = new List<Token>(tokens)
            });

            return tokens;
        }

        private List<Token> ProcessingBrackets(List<Token> tokens)
        {
            while (tokens.Any(t => t.Type == TokenType.LeftBracket))
            {
                // getting data between brackets at the max deep
                var leftBracketIndex = tokens.FindLastIndex(t => t.Type == TokenType.LeftBracket);
                var rightBracketIndex = tokens.FindIndex(leftBracketIndex, t => t.Type == TokenType.RightBracket);
                var tokensBetweenBrackets = tokens.GetRange(leftBracketIndex + 1,
                    rightBracketIndex - leftBracketIndex - 1);

                // If on left is a function, the tokens are function parameters 
                // and must be converted into a decimal list.
                // Otherwise, converting tokens between brackets to decimal number.
                if (leftBracketIndex != 0 && tokens[leftBracketIndex - 1].Type == TokenType.OperatorFunction)
                {
                    var resultParamsList = tokensBetweenBrackets
                        .SplitByListSeparator()
                        .Select(CalculateResultFromTokensWithoutBrackets)
                        .ToList();
                    tokens[leftBracketIndex] = new Token(resultParamsList, CalculatorCultureInfo);
                }
                else
                {
                    var resultNumber = CalculateResultFromTokensWithoutBrackets(tokensBetweenBrackets);
                    tokens[leftBracketIndex] = new Token(resultNumber, CalculatorCultureInfo);
                }
                tokens.RemoveRange(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex);

                _resultInfo.TraceResult.Add(new TraceResultItem
                {
                    Text = "step: processing brackets",
                    Tokens = new List<Token>(tokens)
                });
            }
            return tokens;
        }

        private decimal CalculateResultFromTokensWithoutBrackets(List<Token> tokens)
        {
            var messagesDictionary = new Dictionary<OperatorType, string>
            {
                {OperatorType.Constant, "step: replace constant"},
                {OperatorType.Binary, "step: apply binary operation"},
                {OperatorType.UnaryPrefix, "step: apply prefix operation"},
                {OperatorType.UnaryPostfix, "step: apply postfix operation"},
                {OperatorType.Function, "step: calculate function"}
            };

            // processing all operators in priority order
            foreach (var indexAndOperator in tokens.GetOperatorByPriority())
            {
                tokens.ApplyOperatorAtIndex(indexAndOperator.Key, indexAndOperator.Value, CalculatorCultureInfo);

                _resultInfo.TraceResult.Add(new TraceResultItem
                {
                    Text = messagesDictionary[indexAndOperator.Value.Type],
                    Tokens = new List<Token>(tokens)
                });
            }

            if (tokens.Count != 1)
            {
                throw new FormatException("Wrong expression!");
            }

            var lastToken = tokens.Single();

            if (lastToken.Type != TokenType.Number)
            {
                throw new FormatException("Wrong expression!");
            }

            var result = lastToken.GetNumber();
            return result;
        }
    }
}

// Example expression for calculation: -round(sin(1)10-2.23+zero)+pow(2,3)5!/1
// Answer = 954