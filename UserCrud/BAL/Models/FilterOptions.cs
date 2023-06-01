using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class FilterOptions
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int export { get; set; }
    }
}