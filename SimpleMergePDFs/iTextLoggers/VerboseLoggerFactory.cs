using System;
using iText.IO.Log;
using ILogger = iText.IO.Log.ILogger;

namespace SimpleMergePDFs.iTextLoggers
{
   internal class VerboseLoggerFactory : ILoggerFactory
    {
        private static readonly ILogger Logger = new VerboseLogger();
        public ILogger GetLogger(Type klass)
        {
            return Logger;
        }

        public ILogger GetLogger(string name)
        {
            return Logger;
        }
    }

    public class VerboseLogger : RootLogger
    {
    }
}
