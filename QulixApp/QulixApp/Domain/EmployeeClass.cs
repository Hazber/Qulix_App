using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QulixApp.Domain
{
    [DisplayName("Employee")]
    public class EmployeeClass
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a surname")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter a patronymic")]
        [Display(Name = "Patronymic")]
        public string Patronymic { get; set; }

        public virtual CompanyClass Company { get; set; }

        [UIHint("Enum")]
        [EnumDataType(typeof(Position))]
        [Required(ErrorMessage = "Please select Employee position")]
        [Display(Name = "Position")]
        public Position EmployeePosition { get; set; }

        [HiddenInput(DisplayValue = true)]
        [ScaffoldColumn(false)]
        [Display(Name = "Employment date")]
        public DateTime? EmploymentDate { get; set; }

        public EmployeeClass() { }

        public EmployeeClass(int EmployeeID,string Name, string Surname,string Patronymic,CompanyClass Company,Position EmployeePosition,DateTime? EmploymentDate) 
        {
            this.EmployeeID = EmployeeID;
            this.Name = Name;
            this.Patronymic = Patronymic;
            this.Company = Company;
            this.EmployeePosition = EmployeePosition;
            this.EmploymentDate = EmploymentDate;

        }

    }

    public enum Position
    { 
        [Display(Name ="")]
        None=1,
        Developer=2,
        Tester=3,
        BusinessAnalyst=4,
        Manager=5

    }

    

}