using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ILogger = iText.IO.Log.ILogger;

namespace SimpleMergePDFs.iTextLoggers
{

    public class RootLogger : ILogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public virtual void Warn(string message)
        {
            _logger.Warn(message);
        }

        public virtual bool IsWarnEnabled()
        {
            return true;
        }

        public virtual void Trace(string message)
        {
            _logger.Trace(message);
        }

        public virtual bool IsTraceEnabled()
        {
            return true;
        }

        public virtual void Debug(string message)
        {
            _logger.Debug(message);
        }

        public virtual bool IsDebugEnabled()
        {
            return true;
        }

        public virtual void Info(string message)
        {
            _logger.Info(message);
        }

        public virtual bool IsInfoEnabled()
        {
            return true;
        }

        public virtual void Error(string message)
        {
            _logger.Error(message);
        }

        public virtual  void Error(string message, Exception e)
        {
            _logger.Error(e, message);
        }

        public virtual  bool IsErrorEnabled()
        {
            return true;
        }
    }
}
