using Azure;
using Azure.Communication.Sms;
using BAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace UserCrud.Controllers
{
    public class UserController : Controller

    {
        private readonly IRepository _repository ;
        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: User
        public ActionResult index()
        {
        
            return View();
        }
        [HttpGet]
        public ActionResult UserDetail()
        {
             
            //var userList = _repository.GetUSerDetails();
            return View();
        }
        [HttpPost]
        public ActionResult GetDetails(int pageNumber, int pageSize, string searchValue, int sortColumn, string sortDir)
        {

            var userList = _repository.GetUserDetails(pageNumber , pageSize, searchValue, sortColumn, sortDir);
            return Json(userList);
        }

        [HttpPost]
        public ActionResult UserDetail(UserViewModel model)
        {
             _repository.SaveUserDetails(model);
            var userList = _repository.GetUSerDetails();
            return View(userList);
        }


        
        public ActionResult deleteUserDetail(long userID)
        {
            _repository.deleteUserDetails(userID);

            return RedirectToAction("UserDetail" ,"User");
        }

    }
}
