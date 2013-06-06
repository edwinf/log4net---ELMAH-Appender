log4net---ELMAH-Appender
========================

This appender will allow log4net to be configured to send log messages to ELMAH directly.  This way ELMAH can be the log manager of record for sites while still allowing specific logging parameters to be controlled with log4net as usual.

Sample Log4net config:
<pre><code>
&lt;log4net&gt;
    &lt;appender name="elmahappender" type="elmahappender_log4net.ELMAHAppender, elmahappender_log4net"&gt;
      &lt;layout type="log4net.Layout.PatternLayout"&gt;
        &lt;conversionPattern value="%date [thread] %-5level %logger - %message%newline" /&gt;
      &lt;/layout&gt;
      &lt;UseNullContext&gt;False&lt;/UseNullContext&gt;
    &lt;/appender&gt;
    &lt;root&gt;
      &lt;level value="ALL" /&gt;
      &lt;appender-ref ref="elmahappender" /&gt;
    &lt;/root&gt;
  &lt;/log4net&gt;
  </code></pre>
  My elmah config:
  <pre><code>
  &lt;elmah&gt;
    &lt;errorLog type="Elmah.SqlErrorLog, Elmah" connectionStringName="connString" applicationName="TTDev" /&gt;
  &lt;/elmah&gt;
  </code></pre>
  
  For the sql error log, I had to put the application name on the config.  For v1 I could not figure out how to pass the application name
  through the log4net log, it would ignore what I passed as the app name and just insert an empty string.  The application not matching caused the display handler to not display those log entries.
  Setting the application name in the configuration entry above solved that problem.
  
  This problem did not present itself with the in memory log.

  <b>New for 1.0.2:</b>  A new setting was also added called UseNullContext.  This is used when you want to initialize the logger outside of a HTTP request (such as application start).  
  Please see [this issue](https://github.com/edwinf/log4net---ELMAH-Appender/pull/2) for more details.
  
   <b>Note:</b> A second nuget package was created with the dependancy for log4net dropped to 1.2.10 and .Net 2.0. There were complications getting both 1.2.10 and 1.2.11 dependencies to work in the same package.
   That second package can be found [Here](https://nuget.org/packages/elmahappender_log4net_1.2.10/)
  
