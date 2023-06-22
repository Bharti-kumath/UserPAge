using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserCrud
{
    
    public class LogCustomExceptionFilter :ActionFilterAttribute , IActionFilter 
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
              var responseMessage = "Succesfully Data Rendered" + filterContext.RouteData;
                
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();

            string Message = Environment.NewLine + "Date :" + DateTime.Now.ToString() +
                ", Controller: " + controllerName + ", Action:" + actionName +
                             " Response Message : " + responseMessage;
                                
               
                File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), Message);

               
                
            
        }
    }
}