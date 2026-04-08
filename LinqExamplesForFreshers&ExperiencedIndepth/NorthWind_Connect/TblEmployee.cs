using System;
using System.Collections.Generic;

namespace LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;

public partial class TblEmployee
{
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Salary { get; set; }

    public int DeptId { get; set; }
}
