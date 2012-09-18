using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace elmahappender_log4net
{
    public class ELMAHAppender : log4net.Appender.AppenderSkeleton
    {
		private readonly static Type _DeclaringType = typeof(ELMAHAppender);
		private string _HostName;
		private ErrorLog _ErrorLog;


		public override void ActivateOptions()
		{	
			base.ActivateOptions();
			_HostName = Environment.MachineName;
			try
			{
				this._ErrorLog = ErrorLog.GetDefault(HttpContext.Current);
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
				Error error = new Error(loggingEvent.ExceptionObject);
				error.ApplicationName = ""; //configurable app name?
				error.Detail = base.RenderLoggingEvent(loggingEvent);
				error.HostName = this._HostName;
				error.User = loggingEvent.Identity;
				error.Type = "Custom - " + loggingEvent.Level; // maybe allow the type to be customized?
				//error.Message = loggingEvent.m

				this._ErrorLog.Log(error);
			}
		}
    }
}
