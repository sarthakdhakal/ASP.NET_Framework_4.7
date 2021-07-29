using Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practice.Controllers
{
    public class TestController : Controller
    {
        PracticeDbEntities2 db = new PracticeDbEntities2();

        // GET: Test
        public ActionResult Index()
        {
           
           
            List<Employee> empList= db.Employees.ToList();
            EmployeeViewData employeeViewData = new EmployeeViewData();
            List<EmployeeViewData> employeeViewDatas = empList.Select(x => new EmployeeViewData()
            {
                Name = x.Name,
                EmployeeId = x.EmployeeId,
                Address = x.Address,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.DepartmentName
                

            }).ToList();
            
            
            return View(employeeViewDatas) ;
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

    }
    }
