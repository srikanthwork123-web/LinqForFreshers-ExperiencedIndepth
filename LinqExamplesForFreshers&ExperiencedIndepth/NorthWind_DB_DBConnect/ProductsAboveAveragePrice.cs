using System;
using System.Collections.Generic;

namespace LinqExamplesForFreshers_ExperiencedIndepth.Northwind_DB_DBConnect;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
