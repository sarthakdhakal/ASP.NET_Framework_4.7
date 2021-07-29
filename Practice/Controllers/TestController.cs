using Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

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

            Employee employee = db.Employees.SingleOrDefault(x => x.EmployeeId == employeeId);
            EmployeeViewData employeeViewData = new EmployeeViewData();

            employeeViewData.Name = employee.Name;
            employeeViewData.Address = employee.Address;
            employeeViewData.DepartmentName = employee.Department.DepartmentName;

            return View(employeeViewData);
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
    }
}