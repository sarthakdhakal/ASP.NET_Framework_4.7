using Practice.Models;
using System;
using System.Collections.Generic;
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
           
            EmployeeViewData employeeViewData = new EmployeeViewData();
            List<Employee> employees = db.Employees.ToList();
            List<EmployeeViewData> employeeViewDatas = employees.Select(x => new EmployeeViewData()
            {
                Name = x.Name,
                EmployeeId = x.EmployeeId,
                Address = x.Address,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.DepartmentName
                
            
            }).ToList();
     
            // List<EmployeeViewData> listEmp = db.Employees.Where(x => x.IsDeleted == false).Select(x => new EmployeeViewData { Name = x.Name, DepartmentName = x.Department.DepartmentName, Address = x.Address, EmployeeId = x.EmployeeId }).ToList();
            //
            // ViewBag.EmployeeList = listEmp;
            //
            // return View();

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
            if (ModelState.IsValid)
            {
                Employee employee = new Employee();
                employee.Name = model.Name;
                employee.Address = model.Address;
                employee.DepartmentId = model.DepartmentId;
                db.Employees.Add(employee);
                db.SaveChanges();
                int latestEmpId = employee.EmployeeId;
                Models.Site site = new Models.Site();
                site.name = model.SiteName;
                site.employeeId = latestEmpId;
                db.Sites.Add(site);
                db.SaveChanges();
            
                // return RedirectToAction("Index");
            }

            return View(model);






        }


        public ActionResult EmployeeDetail(int? employeeId)
        {
            if (employeeId == null || employeeId == 0)
            {
                return HttpNotFound();
            }

            List<EmployeeViewData> listEmp = db.Employees.Where(x => x.IsDeleted == false && x.EmployeeId == employeeId).Select(x => new EmployeeViewData { Name = x.Name, DepartmentName = x.Department.DepartmentName, Address = x.Address, EmployeeId = x.EmployeeId }).ToList();

            ViewBag.EmployeeList = listEmp;

            return PartialView("DetailsPartial");
            
        }
        
        public ActionResult Delete(int? employeeId)
        {
            if (employeeId == null || employeeId == 0)
            {
                return HttpNotFound();
            }
        
            var obj=db.Employees.Find(employeeId);
            if (obj == null)
            {
                return HttpNotFound();
            }
        
            db.Employees.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // public JsonResult DeleteEmployee(int EmployeeId)
        // {
        //  
        //
        //     bool result = false;
        //     Employee emp = db.Employees.SingleOrDefault(x => x.IsDeleted == false && x.EmployeeId == EmployeeId);
        //     if (emp != null)
        //     {
        //         emp.IsDeleted = true;
        //         db.SaveChanges();
        //         result = true;
        //     }
        //
        //     return Json(result, JsonRequestBehavior.AllowGet);
        // }
        public ActionResult AddEditEmployee(int EmployeeId)
        {
        
            List<Department> list = db.Departments.ToList();
            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

            EmployeeViewData model = new EmployeeViewData();

            if (EmployeeId > 0) {

                Employee emp = db.Employees.SingleOrDefault(x => x.EmployeeId == EmployeeId );
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