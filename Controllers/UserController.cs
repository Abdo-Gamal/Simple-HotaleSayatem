using HotalManagmentSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;


namespace HotalSystem.Controllers
{
    public class UserController : Controller
    {
        Model context = new Model();

        #region UserEnterface
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAcount()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult Booking()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult services()
        {
            return View();
        }
        public ActionResult Rooms()
        {
            return View();
        }

        #endregion


        // GET: User
        #region user user
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CheckLogin(string UserNationalID,string UserPassword)
        {
            bool Flag = context.Users.Where(c => c.UserNationalID == UserNationalID).Any();
            if (Flag == false)
            {
                string str = "You Can't Enter";
                ViewBag.message = $" {str} ";
                return View("Login");
            }
            else
            {
                var query1 = context.Users.Where(c => c.UserNationalID == UserNationalID).FirstOrDefault();
                if (query1.UserPassword != UserPassword)
                {
                    string str = "Wrong Password";
                    ViewBag.message = $" {str} ";
                    return View("Login");
                }
                return View(query1);
            }
        }


        public ActionResult Search(int id)
        {
            var query = context.Users.FirstOrDefault(u => u.UserID == id);
            return View(query);
        }

        public ActionResult MoreDetails(int id)
        {
            var query = context.RoomAndUsers.Where(d => d.FK_UserID == id).ToList();
            return View(query);
        }
        
        public ActionResult EditUser(int id)
        {
            var query = context.Users.FirstOrDefault(c => c.UserID == id);
            return View(query);
        }
       [HttpPost]
        public ActionResult SaveEditUser(int id, User newuser)
        {

            if (newuser.UserName != null)
            {
                User Euser = context.Users.FirstOrDefault(c => c.UserID == id);
                Euser.UserName = newuser.UserName;
                Euser.UserNationalID = newuser.UserNationalID;
                Euser.UserPassword = newuser.UserPassword;
                Euser.UserPhone = newuser.UserPhone;
                Euser.UserBarthDate = newuser.UserBarthDate;
                context.SaveChanges();
                return RedirectToAction("CheckLogin",new { UserNationalID=newuser.UserNationalID, UserPassword=newuser.UserPassword });
            }
            else
            {
                var query = context.Users.FirstOrDefault(c => c.UserID == id);
                return View("EditUser", query);
            }

        }
    
        #endregion
       
    }
}