using DAL;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace UserCrud
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialise();
            //IRepository repository = DependencyResolver.Current.GetService<IRepository>();
            //HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);

            //SchedulePost postPublish = new SchedulePost();
            //postPublish.Start(repository, httpContext);



        }
           
        


    
}
}
