using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HotalManagmentSystem.Models;
using HotalSystem;
namespace HotalSystem.Controllers
{
    public class AdminRoomController : Controller
    {
        // GET: AdminRoom
        public Model context { get; set; }

        public AdminRoomController()
        {
            context = new Model();
        }
        public ActionResult Index()
        {

            List<Room> Rooms = context.Rooms.ToList();
            List<RoomCategory> Categorys = context.RoomCategorys.ToList();
            foreach (var it in Categorys)
            {
                ViewData[$"{it.CategoryID}"] = it.CategoryNumberOfPerson;

            }
            return View(Rooms);

        }
        public ActionResult Create()
        {
            List<RoomCategory> categorys = context.RoomCategorys.ToList();
            ViewData["Cats"] = categorys;
            
            return View();
        }
        /// <summary>
        /// call create in room AdminRoom
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Room data)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    data.IsReseved = false;
                    List<RoomCategory> categorys = context.RoomCategorys.ToList();
                    ViewData["Cats"] = categorys;
                    RoomCategory category = context.RoomCategorys.Where(r => r.CategoryID == data.FK_CategoryID).FirstOrDefault();
                    category.RoomTypeCount++;
                    category.UnreservedRoomCount++;
                    context.Rooms.Add(data);
                    context.SaveChanges();
                    return RedirectToAction("Index", "AdminRoom");
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            // we can not use RoomCatogry that in Room
            List<RoomCategory> categorys = context.RoomCategorys.ToList();
            ViewData["Cats"] = categorys;
            if (id == 0)
                return RedirectToAction("Index", "AdminRoom");
            Room room = context.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
            return View(room);
        }
        /// <summary>
        /// call function form adminRoomCategoryService 
        /// and show some message
        /// </summary>
        /// <returns
        [HttpPost]
        public ActionResult Edit(int oldCategoryID,int RoomID, Room changedRoom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Room oldRoom = context.Rooms.Where(r => r.RoomID == RoomID).FirstOrDefault();
                    RoomCategory oldCategory = context.RoomCategorys.Where(r => r.CategoryID == oldCategoryID).FirstOrDefault();
                    oldCategory.RoomTypeCount--;
                    oldCategory.UnreservedRoomCount--;

                    RoomCategory newCategory = context.RoomCategorys.Where(r => r.CategoryID == changedRoom.FK_CategoryID).FirstOrDefault();
                    newCategory.RoomTypeCount++;
                    newCategory.UnreservedRoomCount++;

                    oldRoom.FK_CategoryID =changedRoom.FK_CategoryID;
                    //context.Rooms.Attach(changedRoom);
                    //context.Entry(changedRoom).State = System.Data.Entity.EntityState.Modified;
                   
                    int res = context.SaveChanges();
                    if (res > 0)
                    {
                        ViewBag.Success = true;
                        ViewBag.Message = $" Room  ({changedRoom.RoomID}) updated Successful.";
                    }
                    else
                        ViewBag.Message = $"An Error occoured! while update";
                }
                List<RoomCategory> categorys = context.RoomCategorys.ToList();
                ViewData["Cats"] = categorys;
                return View(changedRoom);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }


        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                Room Room = context.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
                return View(Room);
            }
            return RedirectToAction("Index", "AdminRoom");
        }
        [HttpPost]
        public ActionResult DeleteConefirmed(int id)
        {

            Room data = context.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
            if (data.IsReseved == false)
            {
                RoomCategory category = context.RoomCategorys.Where(r => r.CategoryID == data.FK_CategoryID).FirstOrDefault();
                category.RoomTypeCount--;
                category.UnreservedRoomCount--;

                context.Rooms.Remove(data);
                bool ISDeleted = context.SaveChanges() > 0 ? true : false;

                if (ISDeleted)
                {
                    return RedirectToAction("Index", "AdminRoom");
                }
                return RedirectToAction("Delete", "AdminRoom", new { id = id });
              
            }
            ViewBag.Message = "room is reserved cand delete it !";
            return  RedirectToAction("Index", "AdminRoom");
        }


    }
}