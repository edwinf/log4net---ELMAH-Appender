using Elmah;
using System;
using log4net.Appender;
using log4net.Core;
using System.Web;

namespace ElmahAppender.log4net
{
	public class ElmahAppender : AppenderSkeleton
	{
		private readonly static Type _DeclaringType = typeof(ElmahAppender);
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

		protected override void Append(LoggingEvent loggingEvent)
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
				error.Time = DateTime.Now;
				if (loggingEvent.MessageObject != null)
				{
					error.Message = loggingEvent.MessageObject.ToString();
				}
				error.Detail = base.RenderLoggingEvent(loggingEvent);
				error.HostName = this._HostName;
				error.User = loggingEvent.Identity;
				error.Type = "log4net - " + loggingEvent.Level; // maybe allow the type to be customized?
				this._ErrorLog.Log(error);
			}
		}
	}
}
