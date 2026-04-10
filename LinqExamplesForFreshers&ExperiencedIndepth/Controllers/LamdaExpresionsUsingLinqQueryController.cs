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
                                     .Select(g => new { CompanyName = g.Key, CompanyName1 = g.ToList() });
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedCompanyNameData);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("Include usage ")]
        public async Task<IActionResult> IncludeUsage()
        {//Inprogress this example.you can apply pk,fk to employee and department table.

            //Sql Groupby Query:select   CompanyName as CompanyName,Count(*) as Count     from Customers group by CompanyName
            // var employees = context.Employees.Include(e => e.Department).Where(e => e.Department.Name == "HR").ToList();
            //Here you should use the primary key and Foreign key combination then only include will work

            var employees = _northwindDbContext.Employees.Where(e => e.City == "London");
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(employees);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployees&DepartmentDataByUsingJoins")]
        public async Task<IActionResult> GetDataByUsingJoins()
        {
            /*    The below Query is the normal linq query applying  to join
           
            //here we are fetching employess&DepartMenent with data by using joins and orderby descending with required columns.
            //sqlquery:select e.FirstName, e.LastName, e.City, d.DeptName from employee e join Departments d on d.Id=e.EmpId order by e.City desc
            var result = from e in _northwindContext.Employees
                         join d in _northwindContext.Departments
                         on e.EmpId equals d.Id orderby e.City descending
                         select new { e.FirstName, e.LastName, e.City, d.DeptName };
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            */

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
            /*
             * FirstOrDefault() will return data if record exist
             * if data is not exist in database/datasource FirstOrDefault() will return null.
             * ================================================
             * First() method will return the data if record is exist
             * if data is not exist in database/datasource First() will  throw error like InvalidOperationException.
             * ErrorMessage is:System.InvalidOperationException: Sequence contains no elements.
             ===========Note:Multiple Matching records are exist wit same id/name/your searching input  what will happen is?
              *FirstOrDefault()method will not throw any error and it fetch the first matching element in the table.
              *First()method will not throw any error and it fetch the first matching element in the table.

            ====When exactly FirstOrDefault()& First() method usage is===============
            *if  you want to   fetch/get  one  result exists in table then  go for this FirstOrDefault() method
            *if  you want to   fetch/get  one  result exists in table then  go for this First() method will throw error.

            *if record is not exist First() method will throw the error like InvalidOperationException
            *if record is not exist FirstOrDefault() method will return null only.it is not throwing any error.



             */
            //Below Both Queries are same.you can you use any one of the query to fetchdata data based on filter  and return only one record.
            var rm = await _northwindDbContext.Departments.Where(e => e.Deptid == deptid).FirstOrDefaultAsync();

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
            ===========Note:Multiple Matching records are exist wit same id/name/your searching input  what will happen is?
              *SingleOrDefault()method will throw the error like InvalidOperationException.
              *Single() method will throw the error like InvalidOperationException.

            ====When exactly SingleOrDefault()& Single() method usage is===============
            *if you have   exactly only one  result exists in table then only go for this singleordefault()
            *Other wise if you have more than one record matching with your condition singleordefault()&Single() method also will throw error.
            *if record is not exist single() method will throw the error like InvalidOperationException
            *if record is not exist SingleOrDefault() method will return null only.it is not throwing any error.
              */
            //Below Both Queries are same.you can you use any one of the query to fetchdata data based on filter  and return only one record.
            var rm = await _northwindDbContext.Departments.Where(e => e.Deptname == Deptname).FirstOrDefaultAsync();

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
