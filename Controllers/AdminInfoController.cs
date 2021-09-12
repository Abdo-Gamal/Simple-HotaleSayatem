using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotalManagmentSystem.Models;
namespace HotalSystem.Controllers
{
    public class AdminInfoController : Controller
    {
        public Model Context { get; set; }
        public AdminInfoController()
        {
            Context = new Model();
        }
        // GET: AdminInfo
        public ActionResult Index()
        {
            var Admins = Context.Admins.ToList();
            List<Employee> Emps = Context.Employees.ToList();
            foreach (var it in Emps)
            {
                ViewData[$"{it.Employee_ID}"] = it.EmployeeName;
            }
            return View(Admins);
        }
        /// <summary>
        /// call create in room AdminRoom
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            List<Employee> Employees = Context.Employees.ToList();
            ViewData["Emps"] = Employees;
            return View();
        }
        /// <summary>
        /// call create in room AdminRoom
        /// to save in data base
        /// show some worn msge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Admin data)
        {
            try
            {
                List<Employee> Employees = Context.Employees.ToList();
                ViewData["Emps"] = Employees;
                if (ModelState.IsValid)
                {
                    Context.Admins.Add(data);
                    Context.SaveChanges();
                    return RedirectToAction("Index", "AdminInfo");
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
            List<Employee> Employees = Context.Employees.ToList();
            ViewData["Emps"] = Employees;
            if (id == 0)
                return RedirectToAction("Index", "AdminInfo");
            Admin Adm = Context.Admins.Where(A=> A.Admin_ID == id).FirstOrDefault();
            return View(Adm);
        }
        /// <summary>
        /// call function form adminRoomCategoryService 
        /// and show some message
        /// </summary>
        /// <returns
        [HttpPost]
        public ActionResult Edit(Admin changeAdmin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Context.Admins.Attach(changeAdmin);
                    Context.Entry(changeAdmin).State = System.Data.Entity.EntityState.Modified;
                    int res = Context.SaveChanges();
                    if (res > 0)
                    {
                        ViewBag.Success = true;
                        ViewBag.Message = $" Room  ({changeAdmin.Admin_ID}) updated Successful.";
                    }
                    else
                        ViewBag.Message = $"An Error occoured! while update";
                }
                List<Employee> Employees = Context.Employees.ToList();
                ViewData["Emps"] = Employees;
                return View(changeAdmin);
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
                Admin Adman = Context.Admins.Where(A => A.Admin_ID == id).FirstOrDefault();
                return View(Adman);
            }
            return RedirectToAction("Index", "AdminInfo");
        }
        [HttpPost]
        public ActionResult DeleteConefirmed(int id)
        {

            Admin Admin = Context.Admins.Where(a => a.Admin_ID == id).FirstOrDefault();
            Context.Admins.Remove(Admin);
            bool ISDeleted = Context.SaveChanges() > 0 ? true : false;

            if (ISDeleted)
            {
                return RedirectToAction("Index", "AdminInfo");
            }
            return RedirectToAction("Delete", "AdminInfo", new { id = id });
        }
        public ActionResult Details(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "AdminInfo");
            Admin Admin = Context.Admins.Where(a => a.Admin_ID == id).FirstOrDefault();
            Employee EmpInfo = Context.Employees.Where(e => e.Employee_ID == Admin.emb_ID).FirstOrDefault();
            return View(EmpInfo);
        }
    }
}