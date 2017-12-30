using Elmah;
using System;
using log4net.Appender;
using log4net.Core;
using System.Web;

namespace ElmahAppender.log4net
{
    public class ElmahAppender : AppenderSkeleton
    {
        private string _hostName;
        private ErrorLog _errorLog;

        public bool UseNullContext { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            _hostName = Environment.MachineName;
            try
            {
                _errorLog = UseNullContext
                    ? ErrorLog.GetDefault(null)
                    : ErrorLog.GetDefault(HttpContext.Current);
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("Could not create default ELMAH error log", ex);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (_errorLog == null)
                return;

            var error = loggingEvent.ExceptionObject != null
                ? new Error(loggingEvent.ExceptionObject)
                : new Error();

            error.Time = DateTime.Now;
            if (loggingEvent.MessageObject != null)
                error.Message = loggingEvent.MessageObject.ToString();
            error.Detail = RenderLoggingEvent(loggingEvent);
            error.HostName = _hostName;
            error.User = loggingEvent.Identity;
            error.Type = "log4net - " + loggingEvent.Level; // maybe allow the type to be customized?

            _errorLog.Log(error);
        }
    }
}
