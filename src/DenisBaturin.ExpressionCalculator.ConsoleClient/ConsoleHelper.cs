using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using NLog;

namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    public static class ConsoleHelper
    {
        public delegate bool HandlerRoutine(CtrlTypes ctrlType);

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
            [In] [Optional] [MarshalAs(UnmanagedType.FunctionPtr)] HandlerRoutine handler,
            [In] [MarshalAs(UnmanagedType.Bool)] bool add
            );

        public static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            AppLogger.Logger.Log(LogLevel.Trace, "Application closing. ControlType: {0}", ctrlType);
            AppLogger.Logger.Log(LogLevel.Trace, "End application");
            return false;
        }

        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception) e.ExceptionObject;
            Console.WriteLine();
            Console.WriteLine("Observed unhandled exception:");
            Console.WriteLine(exception.Message);
            AppLogger.Logger.Log(LogLevel.Fatal, exception);
            Console.WriteLine();
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}