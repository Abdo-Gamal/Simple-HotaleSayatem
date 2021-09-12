using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;

namespace HotalSystem.Controllers
{
    public class AdminEmployeeController : Controller
    {
        public Model Context { get; set; }
        public AdminEmployeeController()
        {
            Context = new Model();
        }
        // GET: AdminEmployee
        public ActionResult Index()
        {

            var Emps = Context.Employees.ToList();
            return View(Emps);
        }

        /// <summary>
        /// call create in room AdminRoom
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            
            return View();
        }
        /// <summary>
        /// call create in room AdminRoom
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Employee data)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    data.EmployeeImage = SaveImageFile(data.ImageFile);

                    Context.Employees.Add(data);
                    Context.SaveChanges();
                    return RedirectToAction("Index", "AdminEmployee");
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
                return RedirectToAction("Index", "AdminEmployee");
            Employee Emp = Context.Employees.Where(e => e.Employee_ID == id).FirstOrDefault();
            return View(Emp);
        }
        /// <summary>
        /// call function form adminRoomCategoryService 
        /// and show some message
        /// </summary>
        /// <returns
        [HttpPost]
        public ActionResult Edit(Employee changeEmployee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    changeEmployee.EmployeeImage = SaveImageFile(changeEmployee.ImageFile, changeEmployee.EmployeeImage);

                    Context.Employees.Attach(changeEmployee);
                    Context.Entry(changeEmployee).State = System.Data.Entity.EntityState.Modified;
                    int res = Context.SaveChanges();
                    if (res > 0)
                    {
                        ViewBag.Success = true;
                        ViewBag.Message = $" Employee  ({changeEmployee.Employee_ID}) updated Successful.";
                    }
                    else
                        ViewBag.Message = $"An Error occoured! while update";
                }
                return View(changeEmployee);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        private string SaveImageFile(HttpPostedFileBase ImageFile,string oldImage="")
        {
            if (ImageFile != null)
            {
                
                var FileExtantion = Path.GetExtension(ImageFile.FileName);
                var ImageGuid = Guid.NewGuid().ToString();
                string ImageName = ImageGuid + FileExtantion;

                //save
                string filePath = Server.MapPath($"~/Content/Images/{ImageName}");
                ImageFile.SaveAs(filePath);

                //dele old image
                //if (!string.IsNullOrEmpty(oldImage))
                //{
                //    string oldpath = Server.MapPath($"~/Content/Images/{ImageName}");
                //    System.IO.File.Delete(oldpath);
                //}
                return ImageName;
            }
            return oldImage;
        }

        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                Employee Emp = Context.Employees.Where(A => A.Employee_ID == id).FirstOrDefault();
                return View(Emp);
            }
            return RedirectToAction("Index", "AdminEmployee");
        }
        [HttpPost]
        public ActionResult DeleteConefirmed(int id)
        {

            Employee Emp = Context.Employees.Where(a => a.Employee_ID == id).FirstOrDefault();
            Context.Employees.Remove(Emp);
            bool ISDeleted = Context.SaveChanges() > 0 ? true : false;
            if (ISDeleted)
            {
                return RedirectToAction("Index", "AdminEmployee");
            }
            return RedirectToAction("Delete", "AdminEmployee", new { id = id });
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "AdminEmployee");
            Employee emp = Context.Employees.Where(a => a.Employee_ID == id).FirstOrDefault();
            return View(emp);
        }

    }
}