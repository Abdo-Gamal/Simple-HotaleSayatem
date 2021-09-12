using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalSystem.ViewModel;
using HotalSystem.AdminServices;

namespace HotalSystem.Controllers
{
    public class AdminAcountController : Controller
    {
        // GET: AdminAcount
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// //open login form
        /// </summary>
        /// <returns></returns>
        public ActionResult login()
        {
            return View();
        }
        /// <summary>
        /// //check  info  in DB
        /// if have account go to Dashbord
        /// else stay at login form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult login(LoginAdminViewModel LoginInfo)
        {

            var adminservice = new AdminService();
          
            var Islogin = adminservice.login(LoginInfo.Email, LoginInfo.Password);
            if (Islogin == true)
            {

                return RedirectToAction("Index", "AdminDashBord");
            }
            else
            {
                LoginInfo.Massage = "Email or password not correct ";
                return View(LoginInfo);// go to view login() and give it model
            }
        }
    }
}