using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;

namespace HotalSystem.Controllers
{
    public class AdminAndUserController : Controller
    {
        Model context = new Model();
        // GET: AdminAndUser
        public ActionResult Index()
        {
            return View();
        }
         // all uer
        #region User admin Data
        public ActionResult ShowUsersDetils()
        {

            var query = context.Users.ToList();
            return View(query);
        }
		// your Rooms and date 
        public ActionResult DetailsAboutOneUser(int id)
        {
            var query = context.RoomAndUsers.Where(d => d.FK_UserID == id).ToList();

            return View(query);
        }


        public ActionResult SearchOfUserData()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchedData(string UserNationalID)
        {

            bool Flag = context.Users.Where(c => c.UserNationalID == UserNationalID).Any();
            if (Flag == true)
            {
                var query1 = context.Users.FirstOrDefault(c => c.UserNationalID == UserNationalID);

                return View(query1);
            }
            else
            {
                ViewBag.mes = " Wrong iD ";
                return RedirectToAction("ShowUsersDetils");
            }
        }


        public ActionResult Edit(int id)
        {
            var query = context.Users.FirstOrDefault(c => c.UserID == id);
            return View(query);
        }
        [HttpPost]
        public ActionResult SaveEdit(int id, User newuser)
        {

            if (newuser.UserName != null)
            {
                User Euser = context.Users.FirstOrDefault(c => c.UserID == id);
                Euser.UserName = newuser.UserName;
                Euser.UserNationalID = newuser.UserNationalID;
               // Euser.UserPassword = newuser.UserPassword;
                Euser.UserPhone = newuser.UserPhone;
                Euser.UserBarthDate = newuser.UserBarthDate;
                context.SaveChanges();
                return RedirectToAction("ShowUsersDetils");
            }
            else
            {
                var query = context.Users.FirstOrDefault(c => c.UserID == id);
                return View("Edit", query);
            }

        }

        #endregion
    }
}