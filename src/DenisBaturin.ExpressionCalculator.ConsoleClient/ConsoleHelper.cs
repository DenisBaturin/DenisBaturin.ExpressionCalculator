namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using NLog;

    public class ConsoleHelper
    {
        private readonly Logger _logger;

        public delegate bool HandlerRoutine(CtrlTypes ctrlType);

        public ConsoleHelper(Logger logger)
        {
            _logger = logger;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        [DllImport("Kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleCtrlHandler(
            [In][Optional][MarshalAs(UnmanagedType.FunctionPtr)] HandlerRoutine handler,
            [In][MarshalAs(UnmanagedType.Bool)] bool add
            );

        public bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            _logger.Log(LogLevel.Debug, "Application closing. ControlType: {0}", ctrlType);
            _logger.Log(LogLevel.Debug, "End application");
            return false;
        }

        public void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            _logger.Log(LogLevel.Error,
                Environment.NewLine + "Observed unhandled exception:" + Environment.NewLine + exception.Message);
            _logger.Log(LogLevel.Fatal, exception + Environment.NewLine);
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}