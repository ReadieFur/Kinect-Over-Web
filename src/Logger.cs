using System;
using System.Diagnostics;
using System.Reflection;

namespace KinectOverNDI
{
    internal static class Logger
    {
        public static LogLevel logLevel = LogLevel.None;

        public static void Debug(string _message) { WriteLog(LogLevel.Debug, _message); }
        public static void Debug(Exception _ex) { WriteLog(LogLevel.Debug, _ex.Message); }
        public static void Info(string _message) { WriteLog(LogLevel.Info, _message); }
        public static void Info(Exception _ex) { WriteLog(LogLevel.Info, _ex.Message); }
        public static void Warning(string _message) { WriteLog(LogLevel.Warning, _message); }
        public static void Warning(Exception _ex) { WriteLog(LogLevel.Warning, _ex.Message); }
        public static void Error(string _message) { WriteLog(LogLevel.Error, _message); }
        public static void Error(Exception _ex) { WriteLog(LogLevel.Error, _ex.Message); }
        public static void Critical(string _message) { WriteLog(LogLevel.Critical, _message); }
        public static void Critical(Exception _ex) { WriteLog(LogLevel.Critical, _ex.Message); }
        public static void Notice(string _message) { WriteLog(LogLevel.Notice, _message); }
        public static void Notice(Exception _ex) { WriteLog(LogLevel.Notice, _ex.Message); }
        public static void Trace(string _message) { WriteLog(LogLevel.Trace, _message); }
        public static void Trace(Exception _ex) { WriteLog(LogLevel.Trace, _ex.Message); }

        private static void WriteLog(LogLevel _logLevel, string _message)
        {
            if (_logLevel <= logLevel)
            {
                MethodBase stackTraceMethod = new StackTrace().GetFrame(2).GetMethod();
                System.Diagnostics.Debug.WriteLine($"[{_logLevel} @ {DateTime.Now} | {stackTraceMethod.DeclaringType}/{stackTraceMethod.Name}] {_message}");
            }
        }

        public enum LogLevel
        {
            None = 0,
            Debug = 1,
            Info = 2,
            Warning = 3,
            Error = 4,
            Critical = 5,
            Notice = 6,
            Trace = 7
        }
    }
}
