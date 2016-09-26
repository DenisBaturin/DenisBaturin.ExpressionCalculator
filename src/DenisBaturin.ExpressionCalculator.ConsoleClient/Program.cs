using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;

namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    internal class Program
    {
        private const int PromptStringLength = 30;

        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += ConsoleHelper.UnhandledExceptionHandler;
            ConsoleHelper.SetConsoleCtrlHandler(ConsoleHelper.ConsoleCtrlCheck, true);

            AppLogger.Logger.Log(LogLevel.Debug, "Start application");

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyInfo = new AssemblyInfoHelper(executingAssembly);

            Console.Title = assemblyInfo.Name;

            var text = $@"
{assemblyInfo.Product}
{assemblyInfo.Name} (v. {assemblyInfo.Version})
{assemblyInfo.Description}
Enter the expression or special command (e.g. help).
{new string('-', Console.WindowWidth)}";

            AppLogger.Logger.Log(LogLevel.Info, text);

            MainProcessing();

            AppLogger.Logger.Log(LogLevel.Debug, "End application");
        }

        private static void MainProcessing()
        {
            var calculator = new Calculator();

            while (true)
            {
                LogFormattedMessage(LogLevel.Info, "Expression or command:");
                var expression = Console.ReadLine();
                LogFormattedMessage(LogLevel.Debug, expression);

                switch (expression?.Trim())
                {
                    case "help":
                        DisplayHelp();
                        break;
                    case "list":
                        var operators = calculator.GetOperatorsList();
                        var operatorsList = operators.Aggregate(Environment.NewLine,
                            (current, @operator) => current + @operator + Environment.NewLine)
                                            + Environment.NewLine;
                        AppLogger.Logger.Log(LogLevel.Info, operatorsList);
                        continue;
                    case "trace":
                        calculator.TraceMode = !calculator.TraceMode;
                        AppLogger.Logger.Log(LogLevel.Info,
                            "Trace mode is " + calculator.TraceMode + Environment.NewLine);
                        continue;
                    case "correction":
                        calculator.CorrectionMode = !calculator.CorrectionMode;
                        AppLogger.Logger.Log(LogLevel.Info,
                            "Correction mode is " + calculator.CorrectionMode + Environment.NewLine);
                        continue;
                    case "culture":
                        AppLogger.Logger.Log(LogLevel.Info, "Type CultureInfo name: ".PadRight(PromptStringLength));
                        var ciName = Console.ReadLine();
                        if (ciName != null)
                        {
                            CultureInfo cultureInfo;
                            try
                            {
                                cultureInfo = new CultureInfo(ciName);
                            }
                            catch (Exception)
                            {
                                AppLogger.Logger.Log(LogLevel.Info, $"Unsupported CultureInfo: {ciName}");
                                continue;
                            }
                            LogFormattedMessage(LogLevel.Info, "Current CultureInfo:", cultureInfo.EnglishName);
                            LogFormattedMessage(LogLevel.Info, "List separator:", cultureInfo.TextInfo.ListSeparator);
                            LogFormattedMessage(LogLevel.Info, "Number decimal separator:", cultureInfo.NumberFormat.NumberDecimalSeparator);
                            var trace = calculator.TraceMode;
                            var correction = calculator.CorrectionMode;
                            calculator = new Calculator(cultureInfo)
                            {
                                TraceMode = trace,
                                CorrectionMode = correction
                            };
                        }
                        break;
                    case "quit":
                        return;
                    default:
                        try
                        {
                            var resultInfo = calculator.CalculateExpression(expression);

                            if (calculator.TraceMode)
                            {
                                LogFormattedMessage(LogLevel.Info, "");
                                LogFormattedMessage(LogLevel.Info, "<<START TRACE>>");
                                LogFormattedMessage(LogLevel.Info, "Trace mode:", calculator.TraceMode.ToString());
                                LogFormattedMessage(LogLevel.Info, "Correction mode:",
                                    calculator.CorrectionMode.ToString());
                                LogFormattedMessage(LogLevel.Info, "CultureInfo:",
                                    calculator.CalculatorCultureInfo.EnglishName);
                                LogFormattedMessage(LogLevel.Info, "List separator:",
                                    calculator.CalculatorCultureInfo.TextInfo.ListSeparator);
                                LogFormattedMessage(LogLevel.Info, "Number decimal separator:",
                                    calculator.CalculatorCultureInfo.NumberFormat.NumberDecimalSeparator);
                                LogFormattedMessage(LogLevel.Info, "Original expression: ", expression);

                                foreach (var variable in resultInfo.TraceResult)
                                {
                                    variable.Tokens.DisplayAsStringAtConsole(variable.Text.PadRight(PromptStringLength));
                                }

                                LogFormattedMessage(LogLevel.Info, "Answer: ",
                                    resultInfo.Answer.ToString(calculator.CalculatorCultureInfo));
                                LogFormattedMessage(LogLevel.Info, "<<END TRACE>>");
                                LogFormattedMessage(LogLevel.Info, "");
                            }

                            LogFormattedMessage(LogLevel.Info,
                                "Answer:", resultInfo.Answer.ToString(calculator.CalculatorCultureInfo));
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Logger.Log(LogLevel.Info,
                                "Error: ".PadRight(PromptStringLength) + ex.Message);
                            Console.Beep();
                            AppLogger.Logger.Log(LogLevel.Error, ex);
                        }
                        break;
                }

                AppLogger.Logger.Log(LogLevel.Info, new string('-', Console.WindowWidth));
            }
        }

        private static void LogFormattedMessage(LogLevel logLevel, string message1, string message2 = "")
        {
            AppLogger.Logger.Log(logLevel, message1.PadRight(PromptStringLength) + message2);
        }

        private static void DisplayHelp()
        {
            var sb = new StringBuilder(Environment.NewLine);

            sb.AppendLine("Allowed commands:");
            sb.AppendLine();
            sb.AppendLine("help".PadRight(PromptStringLength) + "- Display this manual");
            sb.AppendLine();
            sb.AppendLine("list".PadRight(PromptStringLength) + "- Display list of operators");
            sb.AppendLine();
            sb.AppendLine("trace".PadRight(PromptStringLength) + "- sets mode of trace calculation On/Off");
            sb.AppendLine("Display of all steps of calculating.");
            sb.AppendLine();
            sb.AppendLine("correction".PadRight(PromptStringLength) + "- sets mode of correction expression On/Off");
            sb.AppendLine("Insert the multiplication operator if necessary.");
            sb.AppendLine();
            sb.AppendLine("culture".PadRight(PromptStringLength) + "- Setting CultureInfo");
            sb.AppendLine("For example:");
            sb.AppendLine("ru-RU sets CultureInfo - Russian (Russia).");
            sb.AppendLine("Empty string sets CultureInfo - Invariant Language.");
            sb.AppendLine("CultureInfo by default is Invariant Language (Invariant Country).");
            sb.AppendLine("CultureInfo affects the list separator and the number decimal separator.");
            sb.AppendLine();
            sb.AppendLine("quit".PadRight(PromptStringLength) + "- Quit the application");
            sb.AppendLine();

            AppLogger.Logger.Log(LogLevel.Info, sb.ToString);
        }
    }
}