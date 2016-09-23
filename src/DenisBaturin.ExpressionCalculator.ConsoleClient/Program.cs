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

            AppLogger.Logger.Log(LogLevel.Info, assemblyInfo.Product);
            AppLogger.Logger.Log(LogLevel.Info, $"{assemblyInfo.Name} (v. {assemblyInfo.Version})");
            AppLogger.Logger.Log(LogLevel.Info, assemblyInfo.Description);
            AppLogger.Logger.Log(LogLevel.Info, "Enter the expression or special command (e.g. help).");
            AppLogger.Logger.Log(LogLevel.Info, new string('-', Console.WindowWidth));

            MainProcessing();

            AppLogger.Logger.Log(LogLevel.Debug, "End application");
        }

        private static void MainProcessing()
        {
            var calculator = new Calculator();

            while (true)
            {
                AppLogger.Logger.Log(LogLevel.Info, "Expression or command: ".PadRight(PromptStringLength));
                var expression = Console.ReadLine();
                AppLogger.Logger.Log(LogLevel.Debug, expression);

                switch (expression?.Trim())
                {
                    case "help":
                        DisplayHelp();
                        break;
                    case "list":
                        var operators = calculator.GetOperatorsList();
                        var operatorsList = Environment.NewLine;
                        operatorsList = operators.Aggregate(operatorsList, (current, @operator) => current + (@operator + Environment.NewLine));
                        operatorsList += Environment.NewLine;
                        AppLogger.Logger.Log(LogLevel.Info, operatorsList);
                        continue;
                    case "trace":
                        calculator.TraceMode = !calculator.TraceMode;
                        AppLogger.Logger.Log(LogLevel.Info, "Trace mode is " + calculator.TraceMode + Environment.NewLine);
                        continue;
                    case "correction":
                        calculator.CorrectionMode = !calculator.CorrectionMode;
                        AppLogger.Logger.Log(LogLevel.Info, "Correction mode is " + calculator.CorrectionMode + Environment.NewLine);
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
                                Console.WriteLine($"Unsupported CultureInfo: {ciName}");
                                AppLogger.Logger.Log(LogLevel.Error, $"Unsupported CultureInfo: {ciName}");
                                continue;
                            }
                            AppLogger.Logger.Log(LogLevel.Info, "Current CultureInfo:".PadRight(PromptStringLength) +
                                              $"{cultureInfo.EnglishName}");
                            AppLogger.Logger.Log(LogLevel.Info, "List separator:".PadRight(PromptStringLength) +
                                              $"{cultureInfo.TextInfo.ListSeparator}");
                            AppLogger.Logger.Log(LogLevel.Info, "Number decimal separator:".PadRight(PromptStringLength) +
                                              $"{cultureInfo.NumberFormat.NumberDecimalSeparator}");
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
                                DisplayTraceMessage("");
                                DisplayTraceMessage("<<START TRACE>>");
                                DisplayTraceMessage("Trace mode:", calculator.TraceMode.ToString());
                                DisplayTraceMessage("Correction mode:", calculator.CorrectionMode.ToString());
                                DisplayTraceMessage("CultureInfo:", calculator.CalculatorCultureInfo.EnglishName);
                                DisplayTraceMessage("List separator:", calculator.CalculatorCultureInfo.TextInfo.ListSeparator);
                                DisplayTraceMessage("Number decimal separator:",
                                    calculator.CalculatorCultureInfo.NumberFormat.NumberDecimalSeparator);
                                DisplayTraceMessage("Original expression: ", expression);

                                foreach (var variable in resultInfo.TraceResult)
                                {
                                    variable.Tokens.DisplayAsStringAtConsole(variable.Text.PadRight(PromptStringLength));
                                }

                                DisplayTraceMessage("Answer: ", resultInfo.Answer.ToString(calculator.CalculatorCultureInfo));
                                DisplayTraceMessage("<<END TRACE>>");
                                DisplayTraceMessage("");
                            }

                            AppLogger.Logger.Log(LogLevel.Info, "Answer: ".PadRight(PromptStringLength) + $"{resultInfo.Answer.ToString(calculator.CalculatorCultureInfo)}");
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Logger.Log(LogLevel.Error, "Error: ".PadRight(PromptStringLength) + $"{ex.Message}");
                            Console.Beep();
                            AppLogger.Logger.Log(LogLevel.Error, ex);
                        }
                        break;
                }

                AppLogger.Logger.Log(LogLevel.Info, new string('-', Console.WindowWidth));
            }
        }

        private static void DisplayTraceMessage(string message1, string message2 = "")
        {
            AppLogger.Logger.Log(LogLevel.Trace, $"{message1}".PadRight(PromptStringLength) + $"{message2}");
        }

        private static void DisplayHelp()
        {
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "Allowed commands:");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "help".PadRight(PromptStringLength) + "- Display this manual");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "list".PadRight(PromptStringLength) + "- Display list of operators");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "trace".PadRight(PromptStringLength) + "- sets mode of trace calculation On/Off");
            AppLogger.Logger.Log(LogLevel.Info, "Display of all steps of calculating.");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "correction".PadRight(PromptStringLength) + "- sets mode of correction expression On/Off");
            AppLogger.Logger.Log(LogLevel.Info, "Insert the multiplication operator if necessary.");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "culture".PadRight(PromptStringLength) + "- Setting CultureInfo");
            AppLogger.Logger.Log(LogLevel.Info, "For example:");
            AppLogger.Logger.Log(LogLevel.Info, "ru-RU sets CultureInfo - Russian (Russia).");
            AppLogger.Logger.Log(LogLevel.Info, "Empty string sets CultureInfo - Invariant Language.");
            AppLogger.Logger.Log(LogLevel.Info, "CultureInfo by default is Invariant Language (Invariant Country).");
            AppLogger.Logger.Log(LogLevel.Info, "CultureInfo affects the list separator and the number decimal separator.");
            AppLogger.Logger.Log(LogLevel.Info, "");
            AppLogger.Logger.Log(LogLevel.Info, "quit".PadRight(PromptStringLength) + "- Quit the application");
            AppLogger.Logger.Log(LogLevel.Info, "");
        }
    }
}