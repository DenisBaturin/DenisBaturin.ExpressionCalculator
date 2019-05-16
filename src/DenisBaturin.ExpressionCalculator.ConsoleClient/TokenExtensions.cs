using System;
using System.Collections.Generic;
using DenisBaturin.ExpressionCalculator.Tokens;

namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    internal static class TokenExtensions
    {
        public static void DisplayAsStringAtConsole(this List<Token> tokens, string message)
        {
            var tokenTypeColorsDictionary = new Dictionary<TokenType, ConsoleColor>
            {
                {TokenType.Number, ConsoleColor.White},
                {TokenType.OperatorConstant, ConsoleColor.Blue},
                {TokenType.OperatorFunction, ConsoleColor.Red},
                {TokenType.ParamsList, ConsoleColor.DarkGreen},
                {TokenType.OperatorBinary, ConsoleColor.Yellow},
                {TokenType.OperatorUnaryPostfix, ConsoleColor.Green},
                {TokenType.OperatorUnaryPrefix, ConsoleColor.Cyan},
                {TokenType.ListSeparator, ConsoleColor.DarkGray},
                {TokenType.LeftBracket, ConsoleColor.DarkGray},
                {TokenType.RightBracket, ConsoleColor.DarkGray}
            };

            Console.Write(message);

            Token previousToken = null;

            foreach (var token in tokens)
            {
                Console.ForegroundColor = tokenTypeColorsDictionary[token.Type];
                Console.Write(previousToken != null
                              && token.Type == previousToken.Type
                              && token.Type != TokenType.LeftBracket
                              && token.Type != TokenType.RightBracket
                    ? " " + token
                    : token.ToString());
                previousToken = token;
            }

            Console.ResetColor();
            Console.WriteLine();
        }

    }
}