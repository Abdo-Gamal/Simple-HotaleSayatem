using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;
namespace HotalSystem.Controllers
{
    public class CancelRerservationController : Controller
    {
        // GET: CancelRoom
        Model context;
        public CancelRerservationController()
        {
            context = new Model();
        }
        public ActionResult ShowRooms(int id)
        {
            List<RoomAndUser> roomAndUsers = context.RoomAndUsers.Where(r => r.User.UserID == id).ToList();
            List<string> roomtype = new List<string>();
            List<double> roomprice = new List<double>();
            foreach (RoomAndUser item in roomAndUsers)
            {
                Room room = context.Rooms.Where(r => r.RoomID == item.FK_roomId).FirstOrDefault();
                double price = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryPrice).FirstOrDefault();
                string type = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryNumberOfPerson).FirstOrDefault();
                ViewData[$"{room.RoomID}price"] = price;
                ViewData[$"{room.RoomID}type"] = type;
            }
            return View(roomAndUsers);

        }
        public ActionResult Cancel(int id)
        {
            RoomAndUser roomAndUser = context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();
            RoomCategory category = context.RoomCategorys.Where(r => r.CategoryID == roomAndUser.Room.FK_CategoryID).FirstOrDefault();
            int option = DateTime.Compare(roomAndUser.reservationDate, DateTime.Now);
            if (option != 1)
            {  
                ViewBag.mss = "sorry you can,t edit";
                return View("Index");
            }
            else
            {
                int roomid = roomAndUser.FK_roomId;
                int userid = roomAndUser.FK_UserID;
                context.RoomAndUsers.Remove(roomAndUser);
                context.SaveChanges();
                List<RoomAndUser> listroomanduser = context.RoomAndUsers.ToList();
                bool check_user = false, check_room = false;
                foreach (RoomAndUser item in listroomanduser)
                {
                    if (item.FK_UserID == userid)
                        check_user = true;
                    if (item.FK_roomId == roomid)
                        check_room = true;
                }
                if (check_user == false)
                {
                    context.Users.Remove(context.Users.Where(u => u.UserID == userid).FirstOrDefault());
                }
                if (check_room == false)
                {
                    category.UnreservedRoomCount++;
                    Room room = context.Rooms.Where(r => r.RoomID == roomid).FirstOrDefault();
                    room.IsReseved = false;
                }
                context.SaveChanges();
                ViewBag.message = "Cancel is done successfully";
                return View();
            }
            
        }
        public ActionResult Index()
        {
            return View();
        }
    }


}