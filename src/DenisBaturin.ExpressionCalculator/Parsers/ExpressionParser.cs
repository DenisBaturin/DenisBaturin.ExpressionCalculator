using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DenisBaturin.ExpressionCalculator.Operators;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.Parsers
{
    internal class ExpressionParser
    {
        private readonly CultureInfo _cultureInfo;
        private readonly List<ITokenParser> _parsers;

        public ExpressionParser(
            CultureInfo cultureInfo,
            SpecialSymbolsUniqueList specialSymbols,
            OperatorsUniqueList operators)
        {
            _cultureInfo = cultureInfo;

            _parsers = new List<ITokenParser>
            {
                new NumberTokenParser(),
                new OperatorTokenParser(operators),
                new SpecialSymbolTokenParser(specialSymbols)
            };
        }

        public List<Token> ConvertExpressionToTokens(string expression)
        {
            expression = expression.Trim();
            var tokens = new List<Token>();
            Token leftToken = null;

            var position = 0;
            while (position < expression.Length)
            {
                for (var length = expression.Length - position; length > 0; length--)
                {
                    var subexpression = expression.Substring(position, length);
                    var tempTokens = ReturnTokensByString(subexpression).ToList();

                    Token tempToken;
                    switch (tempTokens.Count)
                    {
                        case 0:
                            if (subexpression==_cultureInfo.NumberFormat.NumberDecimalSeparator)
                            {
                                throw new FormatException("Wrong expression!");
                            }
                            if (length == 1 && subexpression != " ")
                            {
                                throw new ApplicationException($"Unknown token type: {subexpression}");
                            }
                            continue;
                        case 1:
                            tempToken = tempTokens.Single();
                            break;
                        default:
                            if (tempTokens.All(x => x.IsOperatorToken()))
                            {
                                tempToken = ReturnOperatorTokenByLeftToken(leftToken, tempTokens);

                                if (tempToken == null)
                                {
                                    throw new FormatException("Invalid operators order!");
                                }
                            }
                            else
                            {
                                throw new ApplicationException("Ambiguous tokens!");
                            }
                            break;
                    }
                    tokens.Add(tempToken);
                    leftToken = tempToken;
                    position += subexpression.Length - 1;
                    break;
                }

                position++;
            }

            return tokens;
        }

        private IEnumerable<Token> ReturnTokensByString(string expression)
        {
            List<Token> tokens = null;

            foreach (var parser in _parsers)
            {
                tokens = parser.TryParse(expression, _cultureInfo);
                if (tokens.Any())
                {
                    break;
                }
            }

            return tokens;
        }

        private Token ReturnOperatorTokenByLeftToken(Token leftToken, IEnumerable<Token> tokens)
        {
            Token resultToken = null;

            switch (leftToken?.Type)
            {
                case null:
                case TokenType.OperatorUnaryPrefix:
                case TokenType.OperatorBinary:
                case TokenType.LeftBracket:
                case TokenType.ListSeparator:
                    resultToken = tokens
                        .SingleOrDefault(t => t.Type == TokenType.OperatorConstant
                                              || t.Type == TokenType.OperatorFunction
                                              || t.Type == TokenType.OperatorUnaryPrefix);

                    break;
                case TokenType.OperatorConstant:
                case TokenType.OperatorUnaryPostfix:
                case TokenType.RightBracket:
                case TokenType.Number:
                    resultToken = tokens
                        .SingleOrDefault(t => t.Type == TokenType.OperatorConstant
                                              || t.Type == TokenType.OperatorFunction
                                              || t.Type == TokenType.OperatorUnaryPostfix
                                              || t.Type == TokenType.OperatorBinary);

                    break;
            }

            return resultToken;
        }
    }
}
