using LinqExamplesForFreshers_ExperiencedIndepth.Models;
using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;
using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_DB_DBConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;//it contains Jsonconvert class, and  used to serialize and deseriaze the data
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Student = LinqExamplesForFreshers_ExperiencedIndepth.Models.Student;

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
            //our c# compiler converts our linq query to Sql query and excute 
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
            //synatx://var result=from localvariablename in datasource  (optional clause ) select localvariablename   
            //it will return employee data with it department along with all the columns data
            var result = from a in _northwindDbContext.Employees where a.City == "London" select a;//linqquey 
            //SqlQuery:     //select * from  Employees where City='London'

            //here photo column having binary data due to that you are getting refrence loop issue to fix this one below line is used.
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployeesDatawith_Fullname")]
        public async Task<IActionResult> GetrequiredNamesCityWise()
        {//here we are fetchingall the data and showing only one column only.
            //Here new keyword is used to return the object data.
            var result = from a in _northwindDbContext.Employees select new { EmployeeFullName = a.FirstName + a.LastName };
            //SqlQuery Format:select FirstName+LastName as 'EmployeeFullName' from  Employees 

            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetOrderDataWithNamestatswiths")]
        public async Task<IActionResult> GetDataByNamesStartswiths()
        {//here we are fetchingall employess  data with A letter starts employees Data.
            var result = from s in _northwindDbContext.Customers where s.ContactName.StartsWith("A") select s;//select s meaning is fetch the data
            //SQLQUERY:select * from Customers where ContactName like 'A%'

            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployees&DeptDataByUsingJoins")]
        public async Task<IActionResult> GetDataByUsingJoins()
        {
            //here we are fetching employess&DepartMenent with data by using joins and orderby descending with required columns.
            //sqlquery:select e.FirstName, e.LastName, e.City, d.DeptName from employee e join Departments d
            //on d.Id=e.EmpId order by e.City desc
            var result = from e in _northwindContext.Employees
                         join d in _northwindContext.Departments
                         on e.EmpId equals d.Id
                         orderby e.City descending
                         select new { e.FirstName, e.LastName, e.City, d.DeptName };

            //It converts your  objectdata to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("JoinsExampleusingDummyData")]
        public async Task<IActionResult> JoinsExampleusingDummyData()
        {
            //Null values handling with ?? double QuestionMark
            string name = null;

            string resultData = name ?? "Guest";


            //preparing the list of student data with dummy data values(it will store student related data)
            //this data not comming from database.
            //we have created dummy model classes and prepared dummy data and used for Linq queries.
            var students = new List<Student>
            {
            new Student { StudentId = 1, Name = "Alice", CourseId = 101,StudentAddress="hyd" },
            new Student { StudentId = 2, Name = "Bob", CourseId = 102,StudentAddress="chenni" },
             new Student { StudentId = 3, Name = "Charlie", CourseId = 101,StudentAddress="mumbai" }
            };

            var courses = new List<Course>
            {
              new Course { CourseId = 101, CourseName = "Math" },
              new Course { CourseId = 102, CourseName = "Science" }
            };

            /* Key Points
            join... on... equals... is used for inner joins
            into + DefaultIfEmpty() → Left Join
            Query syntax looks like SQL
            Method syntax uses.Join()
            */
            //inner join Linq Query with alias names
            var resultwithAliasNames = from s in students   // _northwindContext.Student  (this is datasource)
                                       join c in courses    //_northwindContext.courses
                         on s.CourseId equals c.CourseId
                         select new
                         {//here you can select required columns
                          //StudentName,CourseName,StudentAddress are the alias names ,you can give any user friendly name.
                             StudentOrginalName = s.Name,
                             CourseFullName = c.CourseName,
                             StudentFullAddress=s.StudentAddress
                         };
            //inner join Linq Query without  alias names
            var resultwithoutAliasNmes = from s in students
                         join c in courses
                         on s.CourseId equals c.CourseId
                         select new
                         {//here you can select required columns 
                          //here not used any alias names.alias names are optional.it is not a mandatory.if you want ,you can use .if you don't want ,you can leave it.
                            s.Name,
                            c.CourseName,
                            s.StudentAddress
                         };
            //same Query we can also write using lamda expressions(method) way (inner join in linq query)
           var LamdaQueryJoinresult = students.Join(
                    courses,
                    s => s.CourseId,        // outer key
                    c => c.CourseId,        // inner key
                    (s, c) => new
                     {
                     StudentName = s.Name,
                     CourseName = c.CourseName
                    });




            //Left Join-Query Syntax (using DefaultIfEmpty)  (into + DefaultIfEmpty() → Left Join)
            //====================================================
            var Leftjoinresult = from s in students
                         join c in courses
                         on s.CourseId equals c.CourseId into sc
                         from c in sc.DefaultIfEmpty() //for left join used this (into  and DefaultIfEmpty() )
                         select new
                         {//here you can fetch required  data
                             StudentName = s.Name,
                             CourseName = c?.CourseName ?? "No Course"
                         };


            //SqlQuery with innerJoins: select s.Name,c.CourName from students s join courses c on s.CourseId = c.CourseId 

            var result = JsonConvert.SerializeObject(resultwithAliasNames);
            var result1 = JsonConvert.SerializeObject(resultwithoutAliasNmes);
            return StatusCode(StatusCodes.Status200OK, result);//here we are return data with statuscode.
        }

        [HttpGet]
        [Route("take(number) Usage")]
        public async Task<IActionResult> TakeUsage()
        {//this is one api method  or action method we are practising the linq queries here.how it will work.

            //if you want to get the only first 5 records in a table use this take(number) method.
            //select top 5 * from customers
            var result = (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(5);

            //if you want to get only first 2 records in a table use this take(2) method.
            var result2= (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(2);


            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("take(2) Usage")]
        public async Task<IActionResult> Take2recordsonly()
        {//this is one api method  or action method we are practising the linq queries here.how it will work.

          

            //if you want to get only first 2 records in a table use this take(2) method.
            var result2 = (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(2);


            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result2);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("Skip(number) Usage")]
        public async Task<IActionResult> SkipUsage()
        {
            //if you want to get the only first 5 records in a table use this take(number) method.
            //after using the take() method you can use skip() method .
            //skip will skip or ignore the given count of records after taking the records.
            //select top 5 * from customers
            /*
             * select * from Employees where City='Seattle'
--where condition is used to filter the data purpose used.
select Top 2 * from Customers
select * from Customers --IT WILL DISPLAY TOTAL DATA
--OFFSET IS USED TO SKIP THE THE RECORDS (OFFSET MEANS SKIP THE DATA)
--FETCH NEXT IS USED TO FETCH THE DATA
SELECT *FROM Customers ORDER BY CustomerId OFFSET 2 ROWS
FETCH NEXT 10 ROWS ONLY;
             * 
             * 
             */
            var result = (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(5).Skip(4);
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("AgeWithFilter")]
        public async Task<IActionResult> AgeWithFilter()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()
            {//List with multiple objects declaring and assigning the data like this way
               new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,//this is one object
               new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 21 } ,//this is one object
               new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,//this is one object
               new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,//this is one object
               new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }//this is one object
            };
            
            var filteredResult = from s in lststudentsObj
                                 where s.Age > 15 && s.Age <= 20
                                 select new { FullName = s.StudentName };//giving the alisaname
             //The above linq query converts into below SqlQuery
             //Select StudentName from StudentData where Age>15 and Age<=20

            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(filteredResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("OrderByusage")]
        public async Task<IActionResult> OrderbyUsage()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()
            {
               new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,
               new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 21 } ,
               new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,
               new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,
               new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }
            };
            var orderByAscendingResult = from s in lststudentsObj
                                         orderby s.StudentName ascending //small values to big values.
                                         select s;

            var orderByDescendingResult = from s in lststudentsObj
                                          orderby s.StudentName descending //big to small values.
                                          select s;
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(orderByDescendingResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusage")]
        public async Task<IActionResult> GroupByusage()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()
            {//IT IS A LIST ,DUE TO THAT IT WILL STORE MULTIPLE STUDENTDATA MODEL CLASS DATA STORE
               new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,
               new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 13 } ,
               new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,
               new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,
               new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }
            };
            //METHOD SYNATX USGE
            var groupedStudents = lststudentsObj.GroupBy(s => s.Age)
                                     .Select(g => new { Age = g.Key, Students = g.ToList() });
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedStudents);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusageWithCount")]
        public async Task<IActionResult> GroupByusageWithCount()
        {
            //example with dummydata
            // Define a list of fruits
            List<string> fruits = new List<string>  //IT WILL STORE STRING TYPE OF DATA ONLY
            {
            "apple", "banana", "orange", "apple", "grape", "banana", "apple"
            };

            // Group the fruits using Query syntax(RealTime Usethis one)
            var groupedFruits = fruits.GroupBy(f => f)
                          .Select(g => new { Fruit = g.Key, Count = g.Count() });

            // Group the fruits using method syntax
            var fruitsGrouped1 = fruits.GroupBy(fruit => fruit);

            // Print the grouped fruits
            foreach (var group in fruitsGrouped1)
            {
                Console.WriteLine($"Fruit: {group.Key}, Count: {group.Count()}");
            }
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedFruits);
            return StatusCode(StatusCodes.Status200OK, convertedData);

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
2. Method syntax (in Mehod syantx we will used lamdaopertor => goes to )
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

Serialization and DeSerialization:
==========================================
Serialization: Serialization means  convert the data from object to json format.
DeSerialization:DeSerialization means convert the data from json to object format.
==================================================================================
=>In DotnetCore To perfomr this Serialization and DeSerialization process,we need to install Newtonsoft.Json package from manage nuget package manager.
=>Serialization:For Serialization process use       var result= Jsonconvert.Serialize(objectname);
=>DeSerialization:For DeSerialization process use   var result=Jsonconvert.Deserializeobject<modelclassname>(jsonstring);




*/


/*
what is the use of ?? in c#

In C#, the ?? operator is called the null-coalescing operator.
It’s used to provide a default value when something is null.

🔹 Basic Syntax
var result = value ?? defaultValue;

👉 Meaning:

If value is NOT null → use value
If value is null → use defaultValue
🔹 Simple Example
=========================
string name = null;

string result = name ?? "Guest";

Console.WriteLine(result);

✅ Output:

Guest
 */
/*
In Microsoft SQL Server, OFFSET and FETCH are used for pagination—i.e., 
skipping a set of rows and returning the next set.

🔹 Basic Syntax
SELECT column1, column2
FROM table_name
ORDER BY column_name
OFFSET <number_of_rows_to_skip> ROWS
FETCH NEXT <number_of_rows_to_return> ROWS ONLY;
🔹 Example Table

Assume a table:

Employees (EmployeeID, Name, Salary)
🔹 Example 1: Skip first 5 rows, fetch next 10
SELECT *
FROM Employees
ORDER BY EmployeeID
OFFSET 5 ROWS
FETCH NEXT 10 ROWS ONLY;

👉 This means:

Skip first 5 rows
Return next 10 rows
🔹 Example 2: First page (no skip)
SELECT *
FROM Employees
ORDER BY EmployeeID
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;
🔹 Example 3: Pagination (Page 3, page size 10)
DECLARE @PageNumber INT = 3;
DECLARE @PageSize INT = 10;

SELECT *
FROM Employees
ORDER BY EmployeeID
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
🔹 Important Rules
ORDER BY is mandatory when using OFFSET FETCH
OFFSET must be ≥ 0
FETCH is optional, but OFFSET is required if FETCH is used
Introduced in SQL Server 2012+
🔹 When to Use
Pagination in applications (web pages, APIs)
Efficient data browsing
Replacing older methods like ROW_NUMBER()
🔹 Alternative (Older Method)

Before SQL Server 2012:

WITH CTE AS (
    SELECT *, ROW_NUMBER() OVER (ORDER BY EmployeeID) AS RowNum
    FROM Employees
)
SELECT *
FROM CTE
WHERE RowNum BETWEEN 6 AND 15;
======================================
*/