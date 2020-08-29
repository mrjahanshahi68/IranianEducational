using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using IrEdu.Web.App_Start;
using System.Data.Entity;
using IrEdu.DataAccess;
using IrEdu.Web.Security;
using IrEdu.DataAccess.Log;
using IrEdu.Web.Cache;
using IrEdu.Web.Log;
using IrEdu.Web.Infrastrcuture;
using System.IO;

namespace IrEdu.Web
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);



			Database.SetInitializer<AppDataContext>(null);
			Database.SetInitializer<LogDataContext>(null);

			SecurityManager.SetProvider(new JwtSecurityProvider());
			CacheManager.SetProvider(new WebCacheProvider());
			LogManager.SetProvider(new List<ILogger> { new FileLogger(), new DatabaseLogger() });

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new AppViewEngine());
		}
	}
}