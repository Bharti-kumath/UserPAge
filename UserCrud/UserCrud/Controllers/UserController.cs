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
           
            return View();
        }
        [HttpPost]
        public ActionResult GetDetails(FilterOptions filterOptions)
        {
           
            var userList = _repository.GetUserDetails(filterOptions);
            return Json(new
            {
                data = userList,
                recordsTotal = userList.FirstOrDefault()?.TotalCount,
                recordsFiltered = userList.FirstOrDefault()?.TotalCount
            });
           
        }

        [HttpPost]
        public ActionResult UserDetail(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveUserDetails(model);
                TempData["SuccessMessage"] = "User added successfully.";
                return RedirectToAction("UserDetail", "User");
            }
            else
            {
                return View(model);
            }
            
        }

        public ActionResult GetUserById(long id)
        {
            var userById = _repository.GetUserById(id);
            return PartialView("UserPartial", userById);
        }
        
        public ActionResult deleteUserDetail(long userID)
        {
            _repository.deleteUserDetails(userID);

            return Json(new {success = true} , JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportCSV()
        {
            _repository.GeCSVFile();

            return Content("CSV Exported"); 
        }

    }
}
