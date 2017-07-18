using System;
using iText.IO.Log;
using ILogger = iText.IO.Log.ILogger;

namespace SimpleMergePDFs.iTextLoggers
{
   internal class DefaultLoggerFactory : ILoggerFactory
    {
        private static readonly ILogger Logger = new DefaultLogger();
        public ILogger GetLogger(Type klass)
        {
            return Logger;
        }

        public ILogger GetLogger(string name)
        {
            return Logger;
        }
    }

    public class DefaultLogger : RootLogger
    {
        public override bool IsDebugEnabled() { return false; }
        public override void Debug(string message) { }

        public override bool IsTraceEnabled() { return false; }
        public override void Trace(string message) { }
    }
}
