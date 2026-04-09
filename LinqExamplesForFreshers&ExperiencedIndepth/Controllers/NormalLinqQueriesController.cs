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


            //preparing the list of student data with dummy values(it will store student related data)
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
            var resultwithAliasNames = from s in students
                         join c in courses
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
            //same Query we can also write using lamda expressions way (inner join in linq query)
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