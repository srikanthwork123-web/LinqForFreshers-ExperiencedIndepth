using System;
using System.Collections.Generic;

namespace LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;

public partial class Department
{
    public long Id { get; set; }

    public string? DeptName { get; set; }

    public virtual ICollection<Employee1> Employee1s { get; set; } = new List<Employee1>();
}
