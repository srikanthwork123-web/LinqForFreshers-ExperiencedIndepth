using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;
using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_DB_DBConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LinqExamplesForFreshers_ExperiencedIndepth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LamdaExpresionsUsingLinqQueryController : ControllerBase
    {
        //In EntityFramework Core To extract the data purpose we used linq queries.
        NorthwindContext _northwindContext;
        NorthwindDbContext _northwindDbContext;
        //INJECT THE DEPENCIES INTO CONSTRUCTOR LIKE THIS WAY
        public LamdaExpresionsUsingLinqQueryController(NorthwindContext northwindContext, NorthwindDbContext northwindDbContext)
        {
            _northwindContext = northwindContext;
            _northwindDbContext = northwindDbContext;
        }

        [HttpGet]
        [Route("GetEmployeesData")]
        //2nd of shortcutfor routing 
        //[HttpGet("GetEmployeesData")]
        //Example: Fetching All Records from employee table example
        public async Task<IActionResult> GetEmployeesData()
        {
            //Basic LamdaLinQ synatx is
            //A lambda expression is written using the => lamda operator
            //lamda expressions will reduce the normal linq query synatx.
            //now a days in realtime we are using this lamda expressions with linq queries(beacuse synatx is very short).
            //SqlQuery:Select * from employees
            //Normal LinqQuery:  var result = from abc in _northwind_DBContext.Employees select abc;
            //the above normal linq query we can also write below way by using Lamda expressions
            //Lamda expression Linq queryis below for fetching data from employee
            var result = _northwindDbContext.Employees.ToList(); // Returns all employees data with all columns.
            //.ToList(); is a method it will return total data of your model class.

            //sqlqueryconverted by compiler:select * from employees
            //the below written for json serialization refrence looping purpose written .net 8.0 to fix this refrence looping we are using this one.lower versions you will not get.
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("GetEmployeesDatawith_CityWise")]
        public async Task<IActionResult> GetAllEmployeesDataCityWise()
        {

//Normal LINQ QUERY:var result = from a in _northwind_DBContext.Employees where a.City == "London" select a;//Normallinqquey 
            //SqlQuery:     //select * from  Employees where City='London'
            //LAMDA EXPRESSION USING LINQ query:
            var result = _northwindDbContext.Employees.Where(a => a.City == "London").ToList();//=>we called lamda opertor
                                                                                                //(parameters) => expression
                                                                                                //here expression is a anoymous function.these functions we used in lamda expressions
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);
            //linq query         // var highEarners = context.Employees.Where(e => e.Salary > 50000).ToList(); 
            // Filters employees with salary > 50,000
            //sqlquery :select * from Employees where Salary > 50000
            //.ToList() means take/fetch the total records.
        }
        [HttpGet]
        [Route("GetEmployeesDatawith_CityWise_MultipleAnd ConditionsUage")]
        public async Task<IActionResult> GetAllEmployeesDataCityWise_MultipleAndConditionsUage()
        {

            //Normal LINQ QUERY:var result = from a in _northwind_DBContext.Employees where a.City == "London"&& a.Country == "UK" && a.Title == "Sales Manager" select a;//linqquey 
            //SqlQuery:     //select * from  Employees where City=='London'and Country == "UK" and Title == "Sales Manager" 
            //LAMDA EXPRESSION USING LINQ query:
            var result = _northwindDbContext.Employees.Where(a => a.City == "London" && a.Country == "UK" && a.Title == "Sales Manager").ToList();//=>we called lamda opertor
                                                                                                                                                   //(parameters) => expression
                                                                                                                                                   //here expression is a anoymous function.these functions we used in lamda expressions
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployeesDatawith_ReuiredColumnsonlyShowing")]
        public async Task<IActionResult> GetEmployeesDatawith_ReuiredColumnsonlyShowing()
        {//here we are fetchingall the data and showing only Required column only.
         //normal linq query:var result = from a in _northwind_DBContext.Employees select new { FirstName, LastName, Address, City };
         //SqlQuery Format:select FirstName+LastName as 'EmployeeFullName' from  Employees 
         //Lamda Expression With Linq query:Thebelow LinQ Query we will get the required colmns only.
            var result = _northwindDbContext.Employees.Select(e => new { e.FirstName, e.LastName, e.Address, e.City }).ToList();
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetOrderDataWithNamestatswiths")]
        public async Task<IActionResult> GetDataByNamesStartswiths()
        {//here we are fetchingall employess  data.
         //normal linq query:var result = from s in _northwind_DBContext.Customers where s.ContactName.StartsWith("A") select s;
         //lamda expression linq query like below.
            var result = _northwindDbContext.Customers.Where(a => a.ContactName.StartsWith("A")).ToList();
            //SQLQUERY:select * from Customers where ContactName like 'A%'
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("OrderByusage")]
        public async Task<IActionResult> OrderbyUsage()
        {
            /*
             //Normal linq queries with ascending order and descending order write like this.
               var orderByAscendingResult = from s in lststudentsObj
                                            orderby s.StudentName ascending
                                            select s;//it will Show the  total columns 
                                                     //Select new{StudentId,Age}//you can also select required columns
               var orderByDescendingResult = from s in lststudentsObj
                                             orderby s.StudentName descending
                                             select s;
            //Here  select s   take The total records.
            //In Lamda Expesions same thing can achieve by .ToList() Predfiend mehod.
            .ToList() means total data retun here.
            */
            //ascending order/descending order  lamda expresion linq query.
            //sqlquery :Select * from Employees order by ContactName
            //sqlquery :Select * from Employees order by ContactName desc
            var orderByAscendingResult = _northwindDbContext.Customers.OrderBy(e => e.ContactName).ToList();//ascending order
            var orderByDescendingResult = _northwindDbContext.Customers.OrderByDescending(e => e.ContactName).ToList();//descending order

            //Order by appling on multile columns combinations.
            //sqlquery :Select * from Employees order by FirstName,LastName

            var orderByonMultipleColumns = _northwindDbContext.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(orderByDescendingResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusageWithOrginalSingleTable")]
        public async Task<IActionResult> GroupByusage()
        {

            //Sql Groupby Query:select   CompanyName as CompanyName,Count(*) as Count     from Customers group by CompanyName
            var groupedCompanyNameData = _northwindDbContext.Customers.GroupBy(s => s.CompanyName)
                                     .Select(g => new { CompanyName = g.Key, Count = g.Count() });
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedCompanyNameData);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("Include() Methodusage")]
        public async Task<IActionResult> IncludeUsage()
        {//Inprogress this example.you can apply pk,fk to employee and department table.

            //Sql Groupby Query:select   CompanyName as CompanyName,Count(*) as Count     from Customers group by CompanyName
            // var employees = context.Employees.Include(e => e.Department).Where(e => e.Department.Name == "HR").ToList();
            //Here you should use the primary key and Foreign key combination then only include will work

            var employees = _northwindDbContext.Employees.Where(e => e.City == "London");//here fetching only city== "London" data.
            //if you want to get the primary key and Foriegn key relation data use .Include(child table call here)method.
            var employeeDepartmentData = _northwindDbContext.Customers.Include(a => a.Orders).ToList();
            //SqlQuery:select * from Customers c inner join Orders o on c.CustomerID=o.CustomerID
            //the below written for json serialization refrence looping purpose written .net 8.0 to fix this refrence looping we are using this one.lower versions you will not get.
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };


            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(employeeDepartmentData, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployees&DepartmentDataByUsingJoins")]
        public async Task<IActionResult> GetDataByUsingJoins()
        {
            /*    The below Query is the normal linq query applying  to join
           
            //here we are fetching employess&DepartMenent with data by using joins and orderby descending with required columns.
            //sqlquery:select e.FirstName, e.LastName, e.City, d.DeptName from employee e join Departments d on d.Id=e.EmpId order by e.City desc
            //Normal Linq query we are writing the inner join Query.
            var result = from e in _northwindContext.Employees
                         join d in _northwindContext.Departments
                         on e.EmployeeId equals d.Deptid orderby e.City descending
                         select new { FirstaName = e.FirstName, DepartmentName = d.Deptname };
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            */
            //above normal linq query and below method synatx linq query both are returning same output.
            //LinQ Query with Lamdaexpressions by using joins.it reduces the join synatx also.
            var employeeDepartments = _northwindDbContext.Employees
                                   .Join(_northwindDbContext.Departments,
                                   e => e.EmployeeId, // Outer key selector
                                   d => d.Deptid,     // Inner key selector
                                   (e, d) => new { FirstaName = e.FirstName, DepartmentName = d.Deptname })
                                   .ToList();//Tolist() will return all the data.
               //sqlquery:select e.FirstName, e.LastName, e.City, d.DeptName from employee e
                  //join Departments d on d.Id=e.EmpId
                    //order by e.City desc


            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(employeeDepartments);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("FirstOrDefault()& First() method usage ")]
        public async Task<IActionResult> FirstOrDefault_First_Usage(int deptid)
        {
            //chatgpturl:https://chatgpt.com/share/673e81cb-7f38-8010-b6e6-4779c0b0db3b
            /*To return Single record purpose we are used FirstOrDefault() and First() methods in Linq.
             * FirstOrDefault() will return data if record exist in the datbase.
             * if data is not exist in database/datasource FirstOrDefault() will return null.
             * ================================================
             * First() method will return the data if record is exist
             * if data is not exist in database/datasource First() will  throw error like System.InvalidOperationException.
             * ErrorMessage is:System.InvalidOperationException: Sequence contains no elements.
             ===========Note:Multiple Matching records are exist wit same id/name/your searching input  what will happen is?
              *FirstOrDefault()method will not throw any error and it fetch the first matching element in the table.
              *First()method will not throw any error and it fetch the first matching element in the table.

            ====When exactly FirstOrDefault()& First() method usage is===============
            *if  you want to   fetch/get  one  result exists in table then  go for this FirstOrDefault() method
            *if  you want to   fetch/get  one  result exists in table then  go for this First() method will throw error.

            *if record is not exist First() method will throw the error like InvalidOperationException
            *if record is not exist FirstOrDefault() method will return null only.it is not throwing any error.

            In realtime we will use FirstOrDefault() method.beacuse if record is not exist ,it will not throw any error.


             */
            //Below Both Queries are same.you can you use any one of the query to fetchdata data based on filter  and return only one record.
            var rm = await _northwindDbContext.Departments.Where(a => a.Deptid == deptid).FirstOrDefaultAsync();

            var rm1 = await _northwindDbContext.Departments.FirstOrDefaultAsync(e => e.Deptid == deptid);
            //==============================usage of First() method ============

            var rm2 = await _northwindDbContext.Departments.FirstAsync(e => e.Deptid == deptid);


            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(rm);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("SingleOrDefault()& Single() method usage ")]
        public async Task<IActionResult> SingleOrDefault_Single_Usage(String Deptname)
        {
            /*
              * SingleOrDefault() will return data if record exist
              * if data is not exist in database/datasource SingleOrDefault() will return null.
              * ================================================
              * Single() method will return the data if record is exist
              * if data is not exist in database/datasource Single() will  throw error like InvalidOperationException.
              * ErrorMessage is:System.InvalidOperationException: Sequence contains no elements.
            ===========Note:Multiple Matching records are exist witH same id/name/your searching input  what will happen is?
              *SingleOrDefault()method will throw the error like System.InvalidOperationException: Sequence contains more than one element.
              *Single() method will throw the error like System.InvalidOperationException: Sequence contains more than one element.
            //In Realtime very leass used SingleOrDefault()& Single() methods 
            //Alway used to fetch single record go with FirstorDefault() method only 
            ====When exactly SingleOrDefault()& Single() method usage is===============
            *if you have   exactly only one  result exists in table then only go for this singleordefault()
            *Other wise if you have more than one record matching with your condition singleordefault()&Single() method also will throw error.
            *if record is not exist single() method will throw the error like InvalidOperationException
            *if record is not exist SingleOrDefault() method will return null only.it is not throwing any error.
              */
            //Below Both Queries are same.you can you use any one of the query to fetchdata data based on filter  and return only one record.
            var rm = await _northwindDbContext.Departments.Where(e => e.Deptname == Deptname).FirstOrDefaultAsync();
            //SAME QUERY WE CAN ASLO WRITE LIKE BELOW WITHOUT WHERE CONDITION
            var rm1 = await _northwindDbContext.Departments.FirstOrDefaultAsync(e => e.Deptname == Deptname);
            //==============================usage of First() method ============
            //The below query you can uncommentt and check the diffrence.
            var rm2 = await _northwindDbContext.Departments.FirstAsync(e => e.Deptname == Deptname);

            //==================SingleOrDefault() and single() method usage============
            //Below Both Queries are same.you can you use any one of the query to fetchdata data based on filter  and return only one record.
            var rm3 = await _northwindDbContext.Departments.Where(e => e.Deptname == Deptname).SingleOrDefaultAsync();

            var rm4 = await _northwindDbContext.Departments.SingleOrDefaultAsync(e => e.Deptname == Deptname);
            //==============================usage of First() method ============

            var rm5 = await _northwindDbContext.Departments.SingleAsync(e => e.Deptname == Deptname);

            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(rm);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

    }
}
/*
.iclude() mehod usage in linq 

The .Include() method is used in LINQ with Entity Framework to eagerly load related data (i.e., navigation properties) along with the main entity.

✅ Why use .Include()?

By default, related data is not loaded (lazy loading or null depending on setup).
.Include() ensures related tables are fetched in the same query.

✅ Example Scenario
🧱 Models (with relationship)
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }  // Navigation property
}

public class Department
{
    public int Id { get; set; }
    public string DeptName { get; set; }

    public List<Employee> Employees { get; set; }
}
✅ Basic .Include() Usage (Method Syntax)
using Microsoft.EntityFrameworkCore;

var employees = context.Employees
                       .Include(e => e.Department)
                       .ToList();
🔍 What happens here?
Fetches Employees
Also fetches related Department data
Generates a SQL JOIN internally
✅ Output Usage Example
foreach (var emp in employees)
{
    Console.WriteLine($"{emp.Name} works in {emp.Department.DeptName}");
}
✅ Multiple .Include() Example
var data = context.Employees
                  .Include(e => e.Department)
                  .Include(e => e.Project)   // another navigation property
                  .ToList();
✅ Nested Include (ThenInclude)

When you have deeper relationships:

var data = context.Employees
                  .Include(e => e.Department)
                  .ThenInclude(d => d.Company)
                  .ToList();
🧠 Key Points
.Include() → loads related entity
.ThenInclude() → loads nested related data

Requires:

using Microsoft.EntityFrameworkCore;
Works only with Entity Framework, not plain LINQ to Objects
❌ Without Include (Problem)
var employees = context.Employees.ToList();

// emp.Department may be NULL or not loaded
✅ With Include (Solution)
var employees = context.Employees
                       .Include(e => e.Department)
                       .ToList();

If you want, I can show:

.Include() with real SQL output
Performance tips (when NOT to use Include)
.Select() vs .Include() differences 👍
please proviide the dummy data  aslo fo rthis one

Here’s a complete working example with dummy data + .Include() usage in Entity Framework (method syntax).

✅ Step 1: Models
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }  // Navigation property
}

public class Department
{
    public int Id { get; set; }
    public string DeptName { get; set; }

    public List<Employee> Employees { get; set; }
}
✅ Step 2: DbContext
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("TestDB"); // Dummy DB
}
✅ Step 3: Insert Dummy Data
using (var context = new AppDbContext())
{
    // Add Departments
    var dept1 = new Department { Id = 1, DeptName = "HR" };
    var dept2 = new Department { Id = 2, DeptName = "IT" };

    context.Departments.AddRange(dept1, dept2);

    // Add Employees
    context.Employees.AddRange(
        new Employee { Id = 1, Name = "Ravi", Department = dept1 },
        new Employee { Id = 2, Name = "Anita", Department = dept2 },
        new Employee { Id = 3, Name = "Kiran", Department = dept2 }
    );

    context.SaveChanges();
}
✅ Step 4: Use .Include() (Method Syntax)
using (var context = new AppDbContext())
{
    var employees = context.Employees
                           .Include(e => e.Department)
                           .ToList();

    foreach (var emp in employees)
    {
        Console.WriteLine($"{emp.Name} works in {emp.Department.DeptName}");
    }
}
✅ Output
Ravi works in HR
Anita works in IT
Kiran works in IT
🔍 What this example shows
Dummy data created using InMemory database
.Include(e => e.Department) loads related department data
Without .Include(), emp.Department may not be populated

If you want next step, I can show:

Real SQL Server version (connection string)
.ThenInclude() with more tables
OR combining .Where + OrderBy + Include in one query

*/
/*======================================
===============orderby usage============

using System;
using System.Collections.Generic;
using System.Linq;



public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
}



class Program
{
    static void Main()
    {
        // Sample data
        List<Employee> employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Ravi", Department = "HR", Salary = 30000 },
            new Employee { Id = 2, Name = "Anita", Department = "IT", Salary = 60000 },
            new Employee { Id = 3, Name = "Kiran", Department = "Finance", Salary = 45000 },
            new Employee { Id = 4, Name = "Sunil", Department = "IT", Salary = 70000 }
        };



        // Order by Name (ascending)
        var sortedByName = employees
                           .OrderBy(e => e.Name)
                           .ToList();



        Console.WriteLine("Order by Name:");
        foreach (var emp in sortedByName)
        {
            Console.WriteLine($"{emp.Name} - {emp.Department} - {emp.Salary}");
        }



        Console.WriteLine();



        // Order by Salary (descending)
        var sortedBySalary = employees
                             .OrderByDescending(e => e.Salary)
                             .ToList();



        Console.WriteLine("Order by Salary (Descending):");
        foreach (var emp in sortedBySalary)
        {
            Console.WriteLine($"{emp.Name} - {emp.Department} - {emp.Salary}");
        }



        Console.WriteLine();



        // Order by Department, then Salary
        var multiSort = employees
                        .OrderBy(e => e.Department)
                        .ThenBy(e => e.Salary)
                        .ToList();



        Console.WriteLine("Order by Department, then Salary:");
        foreach (var emp in multiSort)
        {
            Console.WriteLine($"{emp.Name} - {emp.Department} - {emp.Salary}");
        }
    }
}

===================Description================

OrderBy(e => e.Name) → sorts ascending
OrderByDescending(e => e.Salary) → sorts descending
ThenBy() → applies secondary sorting
ToList() → executes the query
================================
*/