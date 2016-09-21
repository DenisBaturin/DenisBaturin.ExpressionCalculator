using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tokens
{
    internal static class TokensExtensions
    {

        public static IEnumerable<List<Token>> SplitByListSeparator(this List<Token> tokens)
        {
            var partition = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type != TokenType.ListSeparator)
                {
                    partition.Add(token);
                }
                else
                {
                    if (partition.Any())
                    {
                        yield return partition;
                    }
                    partition = new List<Token>();
                }
            }

            if (partition.Any())
            {
                yield return partition;
            }
        }

        public static IEnumerable<KeyValuePair<int, Operator>> GetOperatorByPriority(this List<Token> tokens)
        {
            while (tokens.Any())
            {
                Operator findedOperator = null;
                var findedOperatorIndex = -1;
                for (var i = 0; i < tokens.Count; i++)
                {
                    if (!tokens[i].IsOperatorToken()) continue;
                    var curOperator = tokens[i].GetOperator();
                    // for UnaryPrefix used right associativity
                    if (findedOperatorIndex == -1
                        ||
                        (curOperator.Type == OperatorType.UnaryPrefix
                         && findedOperator != null
                         && curOperator.Priority <= findedOperator.Priority)
                        ||
                        (curOperator.Type != OperatorType.UnaryPrefix
                         && findedOperator != null
                         && curOperator.Priority < findedOperator.Priority))
                    {
                        findedOperatorIndex = i;
                        findedOperator = curOperator;
                    }
                }

                if (findedOperatorIndex == -1 || findedOperator == null)
                {
                    yield break;
                }

                yield return new KeyValuePair<int, Operator>(findedOperatorIndex, findedOperator);
            }
        }

        public static bool IsOperatorToken(this Token token)
        {
            return token.Type == TokenType.OperatorConstant
                   || token.Type == TokenType.OperatorFunction
                   || token.Type == TokenType.OperatorUnaryPostfix
                   || token.Type == TokenType.OperatorBinary
                   || token.Type == TokenType.OperatorUnaryPrefix;
        }

        public static List<Token> ApplyOperatorAtIndex(this List<Token> tokens,
            int operatorIndex, Operator @operator, CultureInfo cultureInfo)
        {
            switch (@operator.Type)
            {
                case OperatorType.Constant:
                    var opConstant = (OperatorConstant) @operator;
                    var resultNumber = opConstant.PerformAction();
                    tokens[operatorIndex] = new Token(resultNumber, cultureInfo);

                    break;
                case OperatorType.UnaryPrefix:
                    if (operatorIndex == tokens.Count - 1
                        || tokens[operatorIndex + 1].Type != TokenType.Number)
                    {
                        throw new FormatException("Wrong expression!");
                    }

                    var rightNumber = tokens[operatorIndex + 1].GetNumber();
                    var opUnaryPrefix = (OperatorUnaryPrefix) @operator;
                    resultNumber = opUnaryPrefix.PerformAction(rightNumber);

                    tokens[operatorIndex] = new Token(resultNumber, cultureInfo);
                    tokens.RemoveAt(operatorIndex + 1);

                    break;
                case OperatorType.Function:
                    if (operatorIndex == tokens.Count - 1
                        || tokens[operatorIndex + 1].Type != TokenType.ParamsList)
                    {
                        throw new FormatException("Wrong expression!");
                    }

                    var args = tokens[operatorIndex + 1].GetParamsArray();
                    var opFunction = (OperatorFunction) @operator;
                    resultNumber = opFunction.PerformAction(args);

                    tokens[operatorIndex] = new Token(resultNumber, cultureInfo);
                    tokens.RemoveAt(operatorIndex + 1);

                    break;
                case OperatorType.UnaryPostfix:
                    if (operatorIndex == 0
                        || tokens[operatorIndex - 1].Type != TokenType.Number)
                    {
                        throw new FormatException("Wrong expression!");
                    }

                    var leftNumber = tokens[operatorIndex - 1].GetNumber();
                    var opUnaryPostfix = (OperatorUnaryPostfix) @operator;
                    resultNumber = opUnaryPostfix.PerformAction(leftNumber);

                    tokens[operatorIndex] = new Token(resultNumber, cultureInfo);
                    tokens.RemoveAt(operatorIndex - 1);

                    break;
                case OperatorType.Binary:
                    if (operatorIndex == 0 || operatorIndex == tokens.Count - 1)
                    {
                        throw new FormatException("Wrong expression!");
                    }

                    if (tokens[operatorIndex - 1].Type != TokenType.Number ||
                        tokens[operatorIndex + 1].Type != TokenType.Number)
                    {
                        throw new FormatException("Wrong expression!");
                    }

                    leftNumber = tokens[operatorIndex - 1].GetNumber();
                    rightNumber = tokens[operatorIndex + 1].GetNumber();
                    var opBinary = (OperatorBinary) @operator;
                    resultNumber = opBinary.PerformAction(leftNumber, rightNumber);

                    tokens[operatorIndex - 1] = new Token(resultNumber, cultureInfo);
                    tokens.RemoveRange(operatorIndex, 2);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(@operator.Type.ToString());
            }

            return tokens;
        }

    }

}