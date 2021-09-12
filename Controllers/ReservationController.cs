using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalSystem.ViewModel;
using HotalManagmentSystem.Models;

namespace HotalSystem.Controllers
{
    public class ReservationController : Controller
    {
        Model context { get; set; }
        public ReservationController()
        {
            context = new Model();
        }
        //[HttpPost]
        public ActionResult Index(int userID)
        {
            User user = context.Users.Where(u => u.UserID == userID).FirstOrDefault();
            ViewBag.user = user;
            List<RoomAndUser> ListOfBookingInfo = context.RoomAndUsers.Where(u => u.FK_UserID == userID).ToList();
            List<string> roomtype = new List<string>();
            List<double> roomprice = new List<double>();
            foreach (RoomAndUser item in ListOfBookingInfo)
            {
                Room room = context.Rooms.Where(r => r.RoomID == item.FK_roomId).FirstOrDefault();
                double price = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryPrice).FirstOrDefault();
                string type = context.RoomCategorys.Where(c => c.CategoryID == room.FK_CategoryID).Select(r => r.CategoryNumberOfPerson).FirstOrDefault();
                ViewData[$"{room.RoomID}price"] = price;
                ViewData[$"{room.RoomID}type"] = type;


            }

            return View(ListOfBookingInfo);
        }

        // GET: Reservation
        public ActionResult Booking()
        {
            List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
            ViewBag.Roomcategories = roomcategories;
            return View();
        }

        [HttpPost]
        public ActionResult Savebooking(BookingdataViewModel BookingData)
        {
            User user = context.Users.Where(u => u.UserNationalID == BookingData.UserNationalID).FirstOrDefault();
           
            ///bool query2 = context.Users.Where(u => u.UserNationalID == BookingData.UserNationalID).Any();
            if (user == null)
            {
                user = new User();
                user.UserName = BookingData.Username;
                user.UserNationalID = BookingData.UserNationalID;
                user.UserPhone = BookingData.UserPhone;
                user.UserBarthDate = BookingData.UserBarthDate;
                user.UserPassword = BookingData.UserPassword;
                context.Users.Add(user); 
            }
            else
            {
                if (BookingData.UserPassword != user.UserPassword)
                {
                    List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
                    ViewBag.Roomcategories = roomcategories;
                    ViewBag.mss = "Error password please try again";
                    return View("Booking");
                }
            }


            List<RoomAndUser> listOfRoomReserved =context.RoomAndUsers.Where(r=>r.Room.FK_CategoryID== BookingData.CatagoryId).ToList();
            DateTime startData = BookingData.reservationDate;

            DateTime EndData = BookingData.reservationDate.AddDays(BookingData.NumberOfDays-1);
            Dictionary<int, int> ValidFRoomReserved = new Dictionary<int, int>();//filter of RoomAndUser 
       
            foreach (RoomAndUser it in listOfRoomReserved)
            {
                
             int option1= DateTime.Compare(it.reservationDate,EndData);//1//your sart bager than me my end
             int option2 = DateTime.Compare(startData,it.reservationDate.AddDays(it.NumberOfDays-1));//1 my start aftar you
               if (option1>0||option2>0)
               {
                   
                    if(ValidFRoomReserved.ContainsKey(it.FK_roomId)==false )
                          ValidFRoomReserved.Add( it.FK_roomId,1);
               }
               else// unvalid
                {
                   if (ValidFRoomReserved.ContainsKey(it.FK_roomId) == true)
                        ValidFRoomReserved[it.FK_roomId]= 0;
                   else  ValidFRoomReserved.Add(it.FK_roomId, 0);
                }
            }
            HashSet<int> uniquevaild= new HashSet<int>();
            foreach (var it in ValidFRoomReserved)
            {
                if(it.Value==1)
                uniquevaild.Add(it.Key);
            }
           // return Content($"{uniquevaild.Count()}");
            int emptyrooms = context.RoomCategorys.Where(d => d.CategoryID == BookingData.CatagoryId).FirstOrDefault().UnreservedRoomCount;
            if (emptyrooms >= BookingData.NumberOfRoom)
            {
                RoomCategory category = context.RoomCategorys.Where(d => d.CategoryID == BookingData.CatagoryId).FirstOrDefault();
                category.UnreservedRoomCount -= BookingData.NumberOfRoom;
               var unreserved = context.Rooms.Where(r => r.IsReseved == false && r.FK_CategoryID == BookingData.CatagoryId).ToList();
               
                foreach(var item in unreserved)//list of room which is unreserved
                {
                    if (BookingData.NumberOfRoom == 0) break;
                    BookingData.NumberOfRoom--;
                    item.IsReseved = true;//room table
                    //room and user
                    RoomAndUser reservationdata = new RoomAndUser();
                    reservationdata.reservationDate = BookingData.reservationDate;
                    reservationdata.NumberOfDays = BookingData.NumberOfDays;
                    reservationdata.FK_UserID = user.UserID;
                    reservationdata.FK_roomId = item.RoomID;
                    //add
                    context.RoomAndUsers.Add(reservationdata);
                }
                //save
                context.SaveChanges();
                // room id
                return RedirectToAction("Index",new { userID = user.UserID });
            }
           else if (emptyrooms+ uniquevaild.Count()>= BookingData.NumberOfRoom)
            {
                RoomCategory category = context.RoomCategorys.Where(d => d.CategoryID == BookingData.CatagoryId).FirstOrDefault();
                category.UnreservedRoomCount -= emptyrooms;// UnreservedRoomCount=emptyrooms
               List< Room> unreserved = context.Rooms.Where(r => r.IsReseved == false && r.FK_CategoryID == BookingData.CatagoryId).ToList();

                foreach (Room item in unreserved)//list of room which is unreserved
                {
                    if (emptyrooms == 0) break;
                    emptyrooms--;
                    BookingData.NumberOfRoom--;
                    item.IsReseved = true;//room table
                    //room and user
                    RoomAndUser reservationdata = new RoomAndUser();
                    reservationdata.reservationDate = BookingData.reservationDate;
                    reservationdata.NumberOfDays = BookingData.NumberOfDays;
                    reservationdata.FK_UserID = user.UserID;
                    reservationdata.FK_roomId = item.RoomID;
                    //add
                    context.RoomAndUsers.Add(reservationdata);
                }
                foreach (int id in uniquevaild)
                {
                   
                    if (BookingData.NumberOfRoom == 0) break;
                    BookingData.NumberOfRoom--;
                    //room and user
                    RoomAndUser reservationdata = new RoomAndUser();
                    reservationdata.reservationDate = BookingData.reservationDate;
                    reservationdata.NumberOfDays = BookingData.NumberOfDays;
                    reservationdata.FK_UserID = user.UserID;
                    reservationdata.FK_roomId = id;
                    //add
                    context.RoomAndUsers.Add(reservationdata);
                }
                    context.SaveChanges();
                return RedirectToAction("Index", new { userID = user.UserID });
            }
            else
            {
                List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
                ViewBag.Roomcategories = roomcategories;
              int totalRoom=  roomcategories.Where(r => r.CategoryID == BookingData.CatagoryId).Select(n => n.RoomTypeCount).FirstOrDefault();
                if (totalRoom < BookingData.NumberOfRoom)
                {
                    ViewBag.message = ($"this numbber of room  not avalible you can rserve {emptyrooms+uniquevaild.Count()} rooms .");
                    
                }
                else
                {
                    ViewBag.message = (emptyrooms != 0 ? $"You Can't Reserve More Than {emptyrooms} ." : $"We do not have any Room in this date !");
                   
                }
                return View("Booking");
            }
        }
      
    }
}