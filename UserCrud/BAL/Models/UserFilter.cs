using System;

public class UserFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int SortColumn { get; set; }
    public string SortDirection { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
