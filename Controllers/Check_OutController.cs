using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;
namespace HotalSystem.Controllers
{
    public class Check_OutController : Controller
    {
        // GET: Check_Out
        Model context { get; set; }
        public Check_OutController()
        {
            context = new Model();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cancel_userInfo()
        {
            return View();
        }
        //[HttpPost]
        public ActionResult Cancel_RoomInfo(string UserNationalID,string s="")
        {

            int userID = context.RoomAndUsers.
                Where(u=>u.User.UserNationalID== UserNationalID).
                Select(u=>u.FK_UserID).FirstOrDefault();

            bool ISuserID = context.RoomAndUsers.
                Where(u => u.User.UserNationalID == UserNationalID).
          Select(u => u.FK_UserID).Any();

            User user = context.Users.Where(u => u.UserID == userID).FirstOrDefault();
            if (userID == 0 || ISuserID == false)
            {
                ViewBag.message = "National ID  is worng please try again";
                return View("Cancel_userInfo");
            }
            if (s != null)
            {
                ViewBag.message = s;
            }
            List<RoomAndUser> ListOfBookingInfo = context.RoomAndUsers.Where(u => u.FK_UserID == userID).ToList();
            List<string> roomtype = new List<string>();
            List<double> roomprice = new List<double>();
            foreach(RoomAndUser item in ListOfBookingInfo)
            {
                Room room = context.Rooms.Where(r => r.RoomID == item.FK_roomId).FirstOrDefault();
                double price = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryPrice).FirstOrDefault();
                string type = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryNumberOfPerson).FirstOrDefault();
                ViewData[$"{room.RoomID}price"] = price;
                ViewData[$"{room.RoomID}type"] = type;

                
            }
         
            ViewBag.user = user;//  ViewBag.user.UserName show  name of user
            return View(ListOfBookingInfo);
            
        }

        [HttpPost]
        public ActionResult CanceledRoomsID(List<int> listRoomId, int UserID)
        {
            User user = context.Users.Where(u => u.UserID == UserID).FirstOrDefault();
            ViewBag.user = user;
            if (listRoomId != null && listRoomId.Count() != 0)
            {

                double price = 0;
                //get uer id 


                List<RoomAndUser> listOfRommAndUserInfo = new List<RoomAndUser>();
                foreach (var it in listRoomId)
                {
                    Room room = context.Rooms.Where(r => r.RoomID == it).FirstOrDefault();
                    RoomCategory roomcategory = context.RoomCategorys.Where(r => r.CategoryID == room.FK_CategoryID).FirstOrDefault();
                   
                       
                    price += context.RoomCategorys.Where(r => r.CategoryID == room.FK_CategoryID)
                        .Select(c => c.CategoryPrice).FirstOrDefault() *
                        context.RoomAndUsers.Where(r => r.FK_roomId == room.RoomID)
                        .Select(c => c.NumberOfDays).FirstOrDefault();

                    //remove fom user and room record
                    RoomAndUser RoomAndUerInfo = context.RoomAndUsers.Where(r => r.FK_roomId == it).FirstOrDefault();
                    if (RoomAndUerInfo == null)
                    {
                        return RedirectToAction("Cancel_RoomInfo", "Check_Out", new { UserNationalID = user.UserNationalID });
                    }
                    //save user and room delete info
                    listOfRommAndUserInfo.Add(RoomAndUerInfo);
                    context.RoomAndUsers.Remove(RoomAndUerInfo);

                   
                }
                context.SaveChanges();
                foreach (var it in listRoomId)
                {
                    bool ISRoomInRoomAndUer = context.RoomAndUsers.Where(r=>r.FK_roomId==it).Any();
                    if (!ISRoomInRoomAndUer)
                    {
                        Room room = context.Rooms.Where(r => r.RoomID == it).FirstOrDefault();
                        RoomCategory roomcategory = context.RoomCategorys.Where(r => r.CategoryID == room.FK_CategoryID).FirstOrDefault();
                        roomcategory.UnreservedRoomCount++;
                        room.IsReseved = false;
                    }
                }
                context.SaveChanges();
                //delete user
                bool IsfoundUser = context.RoomAndUsers.Where(u => u.FK_UserID == UserID).Any();
                if (IsfoundUser == false)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                //save change
                ViewBag.price = price;
                return View(listOfRommAndUserInfo);
            }
            string warning = "Please choose some rooms and try again";
            return RedirectToAction("Cancel_RoomInfo", "Check_Out", new { UserNationalID = user.UserNationalID, s = warning });
        }
    }
}