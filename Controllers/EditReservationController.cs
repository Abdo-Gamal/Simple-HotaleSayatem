using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;
using HotalSystem.ViewModel;
namespace HotalSystem.Controllers
{
    public class EditReservationController : Controller
    {
        // GET: EditReservation
        Model context;
        public EditReservationController() {
            context = new Model();
        }
        
        public ActionResult Show(int id)
        {
            //User user = context.Users.Where(u => u.UserID == id).FirstOrDefault();
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
        public ActionResult Edit(int id)//room and user id
        {
            RoomAndUser roomAndUser = context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();

         
            List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
            ViewBag.Roomcategories = roomcategories;
           
            return View(roomAndUser);  
        }
        public ActionResult SaveEdit(int id,BookingdataViewModel bookingdata)
        {
            RoomAndUser oldroomanduser= context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();
            List<RoomAndUser> roomAndUsers = context.RoomAndUsers.Where(r => r.Room.FK_CategoryID == bookingdata.CatagoryId).ToList();
            int option = DateTime.Compare(oldroomanduser.reservationDate, DateTime.Now);
            if (option != 1)
            {
                RoomAndUser roomAndUser = context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();
                List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
                ViewBag.Roomcategories = roomcategories;
                ViewBag.mss = "sorry you can,t edit";
                return View("Edit", roomAndUser);
            }
            else
            {
                if (oldroomanduser.Room.FK_CategoryID != bookingdata.CatagoryId)
                {
                    //list of all requested rooms

                    int unreserved = context.RoomCategorys.Where(r => r.CategoryID == bookingdata.CatagoryId).Select(c => c.UnreservedRoomCount).FirstOrDefault();
                    if (unreserved >= 1)
                    {
                        RoomCategory roomCategory = context.RoomCategorys.Where(r => r.CategoryID == bookingdata.CatagoryId).FirstOrDefault();
                        roomCategory.UnreservedRoomCount--;
                        int temp = oldroomanduser.FK_roomId;//10
                        Room oldroom = context.Rooms.Where(r => r.RoomID == temp).FirstOrDefault();
                        //new room
                        Room newroom = context.Rooms.Where(r => r.FK_CategoryID == bookingdata.CatagoryId && r.IsReseved == false).FirstOrDefault();
                        newroom.IsReseved = true;
                        oldroomanduser.FK_roomId = newroom.RoomID;//25
                        oldroomanduser.reservationDate = bookingdata.reservationDate;
                        oldroomanduser.NumberOfDays = bookingdata.NumberOfDays;
                        context.SaveChanges();
                        //old room
                        bool isfouundoldroom = context.RoomAndUsers.Where(r => r.FK_roomId == temp).Any();
                        if (isfouundoldroom == false)
                        {
                            RoomCategory oldroomCategory = context.RoomCategorys.Where(r => r.CategoryID == oldroom.FK_CategoryID).FirstOrDefault();
                            oldroomCategory.UnreservedRoomCount++;
                            oldroom.IsReseved = false;

                        }
                        context.SaveChanges();
                        ViewBag.message = "Your edit has done sueccessfully";
                        return View();
                    }
                    else
                    {
                        DateTime startData = bookingdata.reservationDate;
                        DateTime EndData = bookingdata.reservationDate.AddDays(bookingdata.NumberOfDays - 1);
                        Dictionary<int, int> ValidFRoomReserved = new Dictionary<int, int>();//filter of RoomAndUser 

                        foreach (RoomAndUser it in roomAndUsers)
                        {

                            int option1 = DateTime.Compare(it.reservationDate, EndData);//1//your sart bager than me my end
                            int option2 = DateTime.Compare(startData, it.reservationDate.AddDays(it.NumberOfDays - 1));//1 my start aftar you
                            if (option1 > 0 || option2 > 0)
                            {

                                if (ValidFRoomReserved.ContainsKey(it.FK_roomId) == false)
                                    ValidFRoomReserved.Add(it.FK_roomId, 1);
                            }
                            else// unvalid
                            {
                                if (ValidFRoomReserved.ContainsKey(it.FK_roomId) == true)
                                    ValidFRoomReserved[it.FK_roomId] = 0;
                                else ValidFRoomReserved.Add(it.FK_roomId, 0);
                            }
                        }
                        //ashSet<int> uniquevaild = new HashSet<int>();
                        bool flag = false;
                        foreach (var it in ValidFRoomReserved)
                        {
                            if (it.Value == 1)//valied room
                            {
                                int temp = oldroomanduser.FK_roomId;
                                oldroomanduser.FK_roomId = it.Key;//it.key ==>old room id
                                oldroomanduser.reservationDate = bookingdata.reservationDate;
                                oldroomanduser.NumberOfDays = bookingdata.NumberOfDays;
                                context.SaveChanges();
                                //old room
                                bool isfoundoldroom = context.RoomAndUsers.Where(o => o.FK_roomId == temp).Any();
                                Room oldroom = context.Rooms.Where(r => r.RoomID == temp).FirstOrDefault();
                                if (isfoundoldroom == false)
                                {
                                    RoomCategory oldroomCategory = context.RoomCategorys.Where(r => r.CategoryID == oldroom.FK_CategoryID).FirstOrDefault();
                                    oldroomCategory.UnreservedRoomCount++;
                                    oldroom.IsReseved = false;
                                }
                                context.SaveChanges();
                                flag = true;
                                ViewBag.message = "Your edit has done sueccessfully";
                                return View();

                            }

                        }
                        if (!flag)//
                        {
                            ViewBag.mss = "sorry there is no available rooms in this date";
                            RoomAndUser roomAndUser = context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();


                            List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
                            ViewBag.Roomcategories = roomcategories;
                            return View("Edit", roomAndUser);
                        }

                    }
                }
                else
                {
                    DateTime startData = bookingdata.reservationDate;
                    DateTime EndData = bookingdata.reservationDate.AddDays(bookingdata.NumberOfDays - 1);
                    Dictionary<int, int> ValidFRoomReserved = new Dictionary<int, int>();
                    int cnt = 0;
                    foreach (RoomAndUser it in roomAndUsers)
                    {
                        if (it.FK_roomId == oldroomanduser.FK_roomId)
                        {
                            int option1 = DateTime.Compare(it.reservationDate, EndData);//1//your sart bager than me my end
                            int option2 = DateTime.Compare(startData, it.reservationDate.AddDays(it.NumberOfDays - 1));//1 my start aftar you
                            if (option1 > 0 || option2 > 0)
                            {

                                if (ValidFRoomReserved.ContainsKey(it.FK_roomId) == false)
                                    ValidFRoomReserved.Add(it.FK_roomId, 1);
                            }
                            else// unvalid
                            {
                                if (ValidFRoomReserved.ContainsKey(it.FK_roomId) == true)
                                    ValidFRoomReserved[it.FK_roomId] = 0;
                                else ValidFRoomReserved.Add(it.FK_roomId, 0);
                            }
                            cnt++;
                        }
                    }
                    bool flag = false;
                    foreach (var it in ValidFRoomReserved)
                    {
                        if (it.Value == 1)//valied room
                        {
                            oldroomanduser.reservationDate = bookingdata.reservationDate;
                            oldroomanduser.NumberOfDays = bookingdata.NumberOfDays;
                            context.SaveChanges();
                            flag = true;
                            ViewBag.message = "Your edit has done sueccessfully";
                            return View();

                        }

                    }
                    if (cnt == 1)
                    {
                        oldroomanduser.reservationDate = bookingdata.reservationDate;
                        oldroomanduser.NumberOfDays = bookingdata.NumberOfDays;
                        context.SaveChanges();
                        ViewBag.message = "Your edit has done sueccessfully";
                        return View();
                    }
                    if (!flag)//
                    {
                        ViewBag.mss = "sorry there is no available rooms in this date";
                        RoomAndUser roomAndUser = context.RoomAndUsers.Where(r => r.RoomAndUserId == id).FirstOrDefault();


                        List<RoomCategory> roomcategories = context.RoomCategorys.ToList();
                        ViewBag.Roomcategories = roomcategories;
                        return View("Edit", roomAndUser);
                    }

                }
                
            return View();
            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}