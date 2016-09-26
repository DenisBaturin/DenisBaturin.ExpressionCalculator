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
            AppLogger.Logger.Log(LogLevel.Debug, "Application closing. ControlType: {0}", ctrlType);
            AppLogger.Logger.Log(LogLevel.Debug, "End application");
            return false;
        }

        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception) e.ExceptionObject;
            AppLogger.Logger.Log(LogLevel.Error,
                Environment.NewLine + "Observed unhandled exception:" + Environment.NewLine + exception.Message);
            AppLogger.Logger.Log(LogLevel.Fatal, exception + Environment.NewLine);
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}