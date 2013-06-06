using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCTestApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static log4net.ILog _log;

        protected void Application_Start(){
        
            string l4net = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(l4net));
    
            _log = log4net.LogManager.GetLogger(typeof(MvcApplication));


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            _log.Error("Started Application");
        }
    }
}