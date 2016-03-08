using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using NLog;

namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    internal class Program
    {
        private const int PromtStringLenght = 30;

        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += ConsoleHelper.UnhandledExceptionHandler;
            ConsoleHelper.SetConsoleCtrlHandler(ConsoleHelper.ConsoleCtrlCheck, true);

            AppLogger.Logger.Log(LogLevel.Trace, "Start application");

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyInfo = new AssemblyInfoHelper(executingAssembly);
            
            Console.Title = assemblyInfo.Name;

            Console.WriteLine(assemblyInfo.Product);
            Console.WriteLine($"{assemblyInfo.Name} (v. {assemblyInfo.Version})");
            Console.WriteLine(assemblyInfo.Description);
            Console.WriteLine("Enter the expression or special command (e.g. help).");
            Console.WriteLine(new string('-', Console.WindowWidth));

            MainProcessing();

            AppLogger.Logger.Log(LogLevel.Trace, "End application");
        }

        private static void MainProcessing()
        {
            var calculator = new Calculator();

            while (true)
            {
                Console.Write("Expression or command: ".PadRight(PromtStringLenght));
                var expression = Console.ReadLine();
                AppLogger.Logger.Log(LogLevel.Trace, $"Expression or command: {expression}");

                switch (expression?.Trim())
                {
                    case "help":
                        DisplayHelp();
                        break;
                    case "list":
                        Console.WriteLine();
                        var operators = calculator.GetOperatorsList();
                        foreach (var @operator in operators)
                        {
                            Console.WriteLine(@operator);
                        }
                        Console.WriteLine();
                        continue;
                    case "trace":
                        calculator.TraceMode = !calculator.TraceMode;
                        Console.WriteLine("Trace mode is " + calculator.TraceMode);
                        Console.WriteLine();
                        continue;
                    case "correction":
                        calculator.CorrectionMode = !calculator.CorrectionMode;
                        Console.WriteLine("Correction mode is " + calculator.CorrectionMode);
                        Console.WriteLine();
                        continue;
                    case "culture":
                        Console.Write("Type CultureInfo name: ".PadRight(PromtStringLenght));
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
                                AppLogger.Logger.Log(LogLevel.Trace, $"Unsupported CultureInfo: {ciName}");
                                continue;
                            }
                            Console.WriteLine("Current CultureInfo:".PadRight(PromtStringLenght) +
                                              $"{cultureInfo.EnglishName}");
                            Console.WriteLine("List separator:".PadRight(PromtStringLenght) +
                                              $"{cultureInfo.TextInfo.ListSeparator}");
                            Console.WriteLine("Number decimal separator:".PadRight(PromtStringLenght) +
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
                            var answer = calculator.CalculateExpression(expression);
                            Console.WriteLine("Answer: ".PadRight(PromtStringLenght) + $"{answer.ToString(calculator.CalculatorCultureInfo)}");
                            AppLogger.Logger.Log(LogLevel.Trace, $"Answer: {answer.ToString(calculator.CalculatorCultureInfo)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: ".PadRight(PromtStringLenght) + $"{ex.Message}");
                            Console.Beep();
                            AppLogger.Logger.Log(LogLevel.Trace, ex);
                        }
                        break;
                }

                Console.WriteLine(new string('-', Console.WindowWidth));
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Allowed commands:");
            Console.WriteLine();
            Console.WriteLine("help".PadRight(PromtStringLenght) + "- Display this manual");
            Console.WriteLine();
            Console.WriteLine("list".PadRight(PromtStringLenght) + "- Display list of operators");
            Console.WriteLine();
            Console.WriteLine("trace".PadRight(PromtStringLenght) + "- sets mode of trace calculation On/Off");
            Console.WriteLine("Display of all steps of calculating.");
            Console.WriteLine();
            Console.WriteLine("correction".PadRight(PromtStringLenght) + "- sets mode of correction expression On/Off");
            Console.WriteLine("Insert the multiplication operator if necessary.");
            Console.WriteLine();
            Console.WriteLine("culture".PadRight(PromtStringLenght) + "- Setting CultureInfo");
            Console.WriteLine("For example:");
            Console.WriteLine("ru-RU sets CultureInfo - Russian (Russia).");
            Console.WriteLine("Empty string sets CultureInfo - Invariant Language.");
            Console.WriteLine("CultureInfo by default is Invariant Language (Invariant Country).");
            Console.WriteLine("CultureInfo affects the list separator and the number decimal separator.");
            Console.WriteLine();
            Console.WriteLine("quit".PadRight(PromtStringLenght) + "- Quit the application");
            Console.WriteLine();
        }
    }
}