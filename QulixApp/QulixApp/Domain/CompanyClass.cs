using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QulixApp.Domain
{
    [DisplayName("Company")]
    public class CompanyClass
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Please select a company")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a company")]
        public int CompanyID { get; set; }

        [Display(Name ="Company")]
        public string CompanyName { get; set; }

        [UIHint("Enum")]
        [EnumDataType(typeof(OrganizationalForm))]
        [Required(ErrorMessage = "Please select Organizational form")]
        [Display(Name = "Organizational form")]
        public OrganizationalForm CompanyOrganizationalForm { get; set; }

        public CompanyClass() { }

        public CompanyClass(int CompanyID, string CompanyName, OrganizationalForm CompanyOrganizationalForm)
        {
            this.CompanyID = CompanyID;
            this.CompanyName = CompanyName;
            this.CompanyOrganizationalForm = CompanyOrganizationalForm;
        }

    }

    public enum OrganizationalForm
    {
        [Display(Name = "")]
        None=1,
        LLC=2,
        CJSC=3,
        etc=4
    }
}