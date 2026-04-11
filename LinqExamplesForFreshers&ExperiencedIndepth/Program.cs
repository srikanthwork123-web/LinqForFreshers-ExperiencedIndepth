using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_Connect;
using LinqExamplesForFreshers_ExperiencedIndepth.Northwind_DB_DBConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//we must register this context classes ,then only it will work
//when ever run the dotnetcore application it will load the context classes,then only we can get the data by using linq queries
//if you are not registered these context classes it will throw error.
builder.Services.AddDbContext<NorthwindContext>();//NorthwindContext
builder.Services.AddDbContext<NorthwindDbContext>();//NorthwindDbContext
//enable the xml format .if you want to return xml data use below line
builder.Services.AddControllers().AddXmlSerializerFormatters();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
