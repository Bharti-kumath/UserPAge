using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserCrud.Controllers
{
    
    public class HomeController : Controller
    {
       
        public ActionResult ExpiredToken()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            return View();
        }

        public ActionResult Index()
        {
            throw new Exception("Something went wrong");
        }
        public ActionResult About()
        {
            throw new NullReferenceException();
        }
        public ActionResult Contact()
        {
            throw new DivideByZeroException();
        }
    }
}