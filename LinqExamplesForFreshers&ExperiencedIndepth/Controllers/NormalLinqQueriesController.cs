using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;
using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_DB_DBConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
namespace LinqExamplesForFreshers_ExperiencedIndepth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalLinqQueriesController : ControllerBase
    {
        //In EntityFramework Core To extract the data purpose we used linq queries.
        NorthwindContext _northwindContext;
        NorthwindDbContext _northwindDbContext;
       public NormalLinqQueriesController(NorthwindContext northwindContext, NorthwindDbContext northwindDbContext)
        {//add the depencies into constructor of this class
            _northwindContext = northwindContext;
            _northwindDbContext = northwindDbContext;
        }

        [HttpGet]
        [Route("GetEmployeesData")]
        //2nd of shortcutfor routing 
        //[HttpGet("GetEmployeesData")]
        public async Task<IActionResult> GetEmployeesData()
        {
            //Basic LinQ Querysynatx
            //var result=from variablename in datasource  (optional clause ) select variablename
            //here (optional clause ) means where clause,order by clause,Group by Clause...

            //here we are fetchingall employess  data.

            //synatx://var result=from localvariablename in datasource  (optional clause ) select localvariablename
            //Note:abc means its a localvariablename
            var result = from abc in _northwindDbContext.Employees select abc;
            //sqlqueryconverted by compiler:select * from employees
            // Serialize
            string json = JsonSerializer.Serialize(result);
            //Console.WriteLine(json);

            return StatusCode(StatusCodes.Status200OK, json);

        }
    }
}
/* 
 if you want perform any activity(codefirst or dbfirst approach) by uisng entity Framework core we need to install 3 packages mainly.
1. Microsoft.EntityFrameworkCore
2. Microsoft.EntityFrameworkCore.SqlServer
3. Microsoft.EntityFrameworkCore.Tools
All these packages are available in nuget package manager.
all packages use same version for better performance and to avoid any compatibility issues.
To extract the data,we need to use linq queries. 
Linq queries are of two types.
1. Query syntax
2. Method syntax
Query syntax is similar to sql queries and
method syntax is similar to c# programming language.
==========DB FIRST APPROACH IMPLEMENATTION========================
FirstDataBase purpose created Northwind_connect Folder created
For Northwind Datbase model class genertions purpose use this command
=============================================================================================
PM> Scaffold-DbContext "Server=DESKTOP-13B42NJ;Database=Northwind;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Northwind_Connect
==============================================================================================


SecondDataBase purpose created NorthWind_DB_DBConnect Folder created
For Northwind_DB Datbase model class genertions purpose use this command
==========================================================================================
PM> Scaffold-DbContext "Server=DESKTOP-13B42NJ;Database=Northwind_DB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir NorthWind_DB_DBConnect
============================================================================================

*/
