using Practice.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Site = Practice.Models.Site;

namespace Practice.Controllers
{
    public class TestController : Controller
    {
        PracticeDbEntities db = new PracticeDbEntities();

        // GET: Test
        public ActionResult Index()
        {
            // EmployeeViewData employeeViewData = new EmployeeViewData();
            List<Employee> employees = db.Employees.ToList();
            List<EmployeeViewData> employeeViewDatas = employees.Select(x => new EmployeeViewData()
            {
                Name = x.Name,
                EmployeeId = x.EmployeeId,
                Address = x.Address,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.DepartmentName
            }).ToList();


            return View(employeeViewDatas);
        }

        public ActionResult Create()
        {
            List<Department> list = db.Departments.ToList();

            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
           // [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewData model)
        {
            List<Department> list = db.Departments.ToList();

            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");
        
                if (model.EmployeeId > 0)
                {
                    //update
                    Employee emp = db.Employees.SingleOrDefault(x => x.EmployeeId == model.EmployeeId && x.IsDeleted == false);

                    emp.DepartmentId = model.DepartmentId;
                    emp.Name = model.Name;
                    emp.Address = model.Address;
                    db.SaveChanges();


                }
                else {
                    //Insert
                    Employee emp = new Employee();
                    emp.Address = model.Address;
                    emp.Name = model.Name;
                    emp.DepartmentId = model.DepartmentId;
                    emp.IsDeleted = false;
                    db.Employees.Add(emp);
                    db.SaveChanges();
                
                }
                return View(model);
                // return RedirectToAction("Index");
            }

      


        public ActionResult EmployeeDetail(int? employeeId)
        {
            if (employeeId == null || employeeId == 0)
            {
                return HttpNotFound();
            }

            List<EmployeeViewData> listEmp = db.Employees.Where(x => x.IsDeleted == false && x.EmployeeId == employeeId)
                .Select(x => new EmployeeViewData
                {
                    Name = x.Name, DepartmentName = x.Department.DepartmentName, Address = x.Address,
                    EmployeeId = x.EmployeeId
                }).ToList();

            ViewBag.EmployeeList = listEmp;

            return PartialView("DetailsPartial");
        }

        // public ActionResult Delete(int? employeeId)
        // {
        //     if (employeeId == null || employeeId == 0)
        //     {
        //         return HttpNotFound();
        //     }
        //
        //     var obj = db.Employees.Find(employeeId);
        //     if (obj == null)
        //     {
        //         return HttpNotFound();
        //     }
        //
        //     try
        //     {
        //         db.Employees.Remove(obj);
        //         db.SaveChanges();
        //         return RedirectToAction("Index");
        //     }
        //     catch (SqlException e)
        //     {
        //         return RedirectToAction("Index");
        //     }
        // }

     
        public JsonResult Delete(int employeeId)
            {

            var siteObj = db.Sites.Where(x => x.employeeId == employeeId).FirstOrDefault();
            if (siteObj != null)
            {
                int siteid = siteObj.id;
             
                var obj1 = db.Sites.Find(siteid);
                db.Sites.Remove(obj1);
                db.SaveChanges();
            }
            var obj = db.Employees.Find(employeeId);
            System.Diagnostics.Debug.WriteLine(obj+"hERE");

           
                try
                {
       
                    db.Employees.Remove(obj );
                    db.SaveChanges();
                }
            
                    catch (Exception e)
                    {
                        const bool result = false;
                        return Json(new {result, error = e.Message});
                    }

                    // bool result = false;
                    // Employee emp = db.Employees.SingleOrDefault(x => x.IsDeleted == false && x.EmployeeId == EmployeeId);
                    // if (emp != null)
                    // {
                    //     emp.IsDeleted = true;
                    //     db.SaveChanges();
                    //     result = true;
                    // }
                    const bool condition = true;
                    return Json(new { condition, success= "Deleted successfully" });

        }
        

        public ActionResult AddEditEmployee(int EmployeeId)
        {
            List<Department> list = db.Departments.ToList();
            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

            EmployeeViewData model = new EmployeeViewData();

            if (EmployeeId > 0)
            {
                Employee emp = db.Employees.SingleOrDefault(x => x.EmployeeId == EmployeeId);
                model.EmployeeId = emp.EmployeeId;
                model.DepartmentId = emp.DepartmentId;
                model.Name = emp.Name;
                model.Address = emp.Address;
                Site site = db.Sites.SingleOrDefault(x => x.employeeId == EmployeeId);
                model.SiteName = site.name;
            }


            return PartialView("AddEdit", model);
        }
    }
}