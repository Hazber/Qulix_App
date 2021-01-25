using QulixApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QulixApp.Models
{
    public class EmployeeModel
    {
        public EmployeeClass Employee { get; set; }
        private IList<CompanyClass> Company { get; set; }

        public EmployeeModel() { }
        public EmployeeModel(EmployeeClass employee, IList<CompanyClass> company)
        {
            this.Employee = employee;
            this.Company = company;
        }

        public IEnumerable<SelectListItem> SelectCompany()
        {
            if (Company != null) return new SelectList(Company, "CompanyID", "CompanyName","CompanyOrganizationalForm");
            return null;
        }
    }
}