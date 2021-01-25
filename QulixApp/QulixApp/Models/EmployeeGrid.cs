using QulixApp.Domain;
using System.Collections.Generic;


namespace QulixApp.Models
{
    public class EmployeeGrid
    {
        public IEnumerable<EmployeeClass> Employee { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public SortingInfo SortingInfo { get; set; }
    }
}