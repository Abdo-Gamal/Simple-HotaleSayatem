using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;

namespace HotalSystem.Controllers
{
    
    public class RoomCategoriesController : Controller
    {
        Model context = new Model();
        // GET: RoomCategories
     
        public ActionResult DetailsAboutOneRoomCategorie(int id)
        {

            var query = context.RoomCategorys.FirstOrDefault(ID => ID.CategoryID == id);
            return View(query);
        }

        public ActionResult ShowAllCategoryDetails()
        {

            List<RoomCategory> list = context.RoomCategorys.ToList();
            return View(list);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}