log4net---ELMAH-Appender
========================

This appender will allow log4net to be configured to send log messages to ELMAH directly.  This way ELMAH can be the log manager of record for sites while still allowing specific logging parameters to be controlled with log4net as usual.

Sample Log4net config:
<log4net>
    <appender name="elmahappender" type="elmahappender_log4net.ELMAHAppender, elmahappender_log4net">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
    <root>
      <level value="ALL" />
      <appender-ref ref="elmahappender" />
    </root>
  </log4net>
  
  My elmah config:
  
  <elmah>
    <errorLog type="Elmah.SqlErrorLog, Elmah" connectionStringName="connString" applicationName="TTDev" />
  </elmah>
  
  Note: For the sql error log, I had to put the application name on the config.  For v1 I could not figure out how to pass the application name
  through the log4net log, it would ignore what I passed as the app name and just insert an empty string.  The application not matching caused the display handler to not display those log entries.
  Setting the application name in the configuration entry above solved that problem.
  
  This problem did not present itself with the in memory log.