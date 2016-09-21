using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DenisBaturin.ExpressionCalculator.OperatorDefinitions;

namespace DenisBaturin.ExpressionCalculator.Tokens
{
    public class Token
    {
        private readonly CultureInfo _cultureInfo;
        private readonly object _value;

        public Token(decimal number, CultureInfo cultureInfo)
        {
            _value = number;
            Type = TokenType.Number;
            _cultureInfo = cultureInfo;
        }

        public Token(Operator @operator, CultureInfo cultureInfo)
        {
            _value = @operator;
            Type = GetTokenTypeByOperatorType(@operator.Type);
            _cultureInfo = cultureInfo;
        }

        public Token(ISpecialSymbol specialSymbol, CultureInfo cultureInfo)
        {
            _value = specialSymbol;
            Type = GetTokenTypeBySpecialSymbolType(specialSymbol.Type);
            _cultureInfo = cultureInfo;
        }

        public Token(List<decimal> args, CultureInfo cultureInfo)
        {
            _value = args;
            Type = TokenType.ParamsList;
            _cultureInfo = cultureInfo;
        }

        public TokenType Type { get; }

        public decimal GetNumber()
        {
            if (Type != TokenType.Number)
            {
                throw new ArgumentException("Token is not a number! " + this);
            }
            return (decimal) _value;
        }

        public List<decimal> GetParamsArray()
        {
            if (Type != TokenType.ParamsList)
            {
                throw new ArgumentException("Token is not a params array! " + this);
            }
            return (List<decimal>) _value;
        }

        public Operator GetOperator()
        {
            if (!this.IsOperatorToken())
            {
                throw new ArgumentException("Token is not a operator! " + this);
            }
            return _value as Operator;
        }

        private TokenType GetTokenTypeByOperatorType(OperatorType operatorType)
        {
            TokenType result;

            switch (operatorType)
            {
                case OperatorType.Constant:
                    result = TokenType.OperatorConstant;
                    break;
                case OperatorType.Binary:
                    result = TokenType.OperatorBinary;
                    break;
                case OperatorType.Function:
                    result = TokenType.OperatorFunction;
                    break;
                case OperatorType.UnaryPostfix:
                    result = TokenType.OperatorUnaryPostfix;
                    break;
                case OperatorType.UnaryPrefix:
                    result = TokenType.OperatorUnaryPrefix;
                    break;
                default:
                    throw new ArgumentException("Wrong OperatorType!");
            }

            return result;
        }

        private TokenType GetTokenTypeBySpecialSymbolType(SpecialSymbolType specialSymbolType)
        {
            TokenType result;

            switch (specialSymbolType)
            {
                case SpecialSymbolType.LeftBracket:
                    result = TokenType.LeftBracket;
                    break;
                case SpecialSymbolType.RightBracket:
                    result = TokenType.RightBracket;
                    break;
                case SpecialSymbolType.ListSeparator:
                    result = TokenType.ListSeparator;
                    break;
                default:
                    throw new ArgumentException("Wrong SpecialSymbolType!");
            }

            return result;
        }

        public override string ToString()
        {
            string result;
            switch (Type)
            {
                case TokenType.Number:
                    result = ((decimal) _value).ToString(_cultureInfo);
                    break;
                case TokenType.LeftBracket:
                case TokenType.RightBracket:
                case TokenType.ListSeparator:
                    result = ((SpecialSymbol) _value).StringView;
                    break;
                case TokenType.OperatorConstant:
                case TokenType.OperatorBinary:
                case TokenType.OperatorUnaryPostfix:
                case TokenType.OperatorUnaryPrefix:
                case TokenType.OperatorFunction:
                    result = ((Operator) _value).StringView;
                    break;
                case TokenType.ParamsList:
                    result = string.Join(_cultureInfo.TextInfo.ListSeparator,
                        ((List<decimal>) _value).Select(num => num.ToString(_cultureInfo)));
                    result = "[" + result + "]";
                    break;
                default:
                    throw new ArgumentException("Wrong TokenType!");
            }

            return result;
        }
    }
}