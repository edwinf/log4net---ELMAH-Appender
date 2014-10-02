using Elmah;
using System;
using System.Web;

namespace Hsb.Log4Net.Appender
{
    public class ELMAHAppender : log4net.Appender.AppenderSkeleton
    {
        private string _HostName;
        private ErrorLog _ErrorLog;

        public bool UseNullContext { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            _HostName = Environment.MachineName;
            try
            {
                if (UseNullContext)
                {
                    this._ErrorLog = ErrorLog.GetDefault(null);
                }
                else
                {
                    this._ErrorLog = ErrorLog.GetDefault(HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error("Could not create default ELMAH error log", ex);
            }
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (this._ErrorLog != null)
            {
                Error error;
                if (loggingEvent.ExceptionObject != null)
                {
                    error = new Error(loggingEvent.ExceptionObject);
                }
                else
                {
                    error = new Error();
                }
                error.Time = loggingEvent.TimeStamp;
                if (loggingEvent.MessageObject != null)
                {
                    error.Message = loggingEvent.MessageObject.ToString();
                }
                error.Detail = base.RenderLoggingEvent(loggingEvent);
                error.HostName = this._HostName;
                error.User = loggingEvent.Identity;
                error.Source = loggingEvent.LoggerName;
                error.Type = loggingEvent.Level.Name;
                this._ErrorLog.Log(error);
            }
        }
    }
}