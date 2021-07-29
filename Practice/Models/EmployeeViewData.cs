using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class EmployeeViewData
    {
        
        public int EmployeeId { get; set; }
        [Required(ErrorMessage="Name is necessary")]
        public string Name { get; set; }
       
        public Nullable<int> DepartmentId { get; set; }
        [Required(ErrorMessage="Address is necessary")]
        public string Address { get; set; }
        
        public virtual Department Department { get; set; }
   
        public string DepartmentName { get; internal set; }
        [Required(ErrorMessage="Site Name is necessary")]
        public string SiteName { get; set; }

    }
}