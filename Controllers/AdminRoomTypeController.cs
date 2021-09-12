using HotalSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;

namespace HotalSystem.Controllers
{
    public class AdminRoomTypeController : Controller
    {
        private readonly AdminRoomCategoryService CategoryService;

        public AdminRoomTypeController()
        {
            CategoryService = new AdminRoomCategoryService();
        }
        // GET: AdminDashBord

        /// <summary>
        /// show all room category and eidet and remove and details 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
           List<RoomCategory> Categorys= CategoryService.ReadAll();
            return View(Categorys);
        }
        /// <summary>
        /// open form for add room category
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
          
            return View();
        }
        /// <summary>
        /// call create in room AdminRoomCategoryService 
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(RoomCategory data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int rest = CategoryService.Create(data);
                    if (rest==-2)
                    {
                        ViewBag.Massege ="Catogry aready Eixsist ";
                        return View(data);
                    }
                   return RedirectToAction("Index", "AdminRoomType");
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// open form for Edit all proparty for room category
        /// </summary>
        /// <returns
        public ActionResult Edit(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "AdminRoomType");

            RoomCategory Category = CategoryService.ReadbyId(id);
            return View(Category);
        }
        /// <summary>
        /// call function form adminRoomCategoryService 
        /// and show some message
        /// </summary>
        /// <returns
        [HttpPost]
        public ActionResult Edit(RoomCategory changedCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = CategoryService.Eidet(changedCategory);
                  
                    if (res == -1)
                        ViewBag.Message = $"you can not delet room as it reserved now";

                    else if (res >= 0)
                    {
                        ViewBag.Success = true;
                        ViewBag.Message = $" Category  ({changedCategory.CategoryID}) updated Successful.";
                    }
                    else
                        ViewBag.Message = $"An Error occoured! while update";
                }
                //return Content($"{ModelState.ToString()}");
              return View(changedCategory);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
        /// <summary>
        /// show discraption of room 
        /// and price 
        /// </summary>
        /// <returns
        public ActionResult Details(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "AdminRoomType");

            List<RoomCategory> Categorys = CategoryService.ReadAll();
            var Category = Categorys.Where(c=>c.CategoryID==id).FirstOrDefault();
            return View(Category);
        }

        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                

                List<RoomCategory> Categorys = CategoryService.ReadAll();
                var Category = Categorys.Where(c => c.CategoryID == id).FirstOrDefault();
                return View(Category);
            }
            return RedirectToAction("Index", "AdminRoomType");
        }
        [HttpPost]
        public ActionResult  DeleteConefirmed(int id)
        {
            bool ISDeleted=CategoryService.Delete(id);
            if (ISDeleted)
            {
                return RedirectToAction("Index", "AdminRoomType");
            }
            return RedirectToAction("Delete", "AdminRoomType", new { id=id});
        }
    }
}