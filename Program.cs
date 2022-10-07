using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EmployeesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EmployeeDB>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Employees API",
         Description = "Employee List",
         Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employees API V1");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/employees", async (EmployeeDB db) => await db.Employees.ToListAsync());
app.MapPost("/employees", async (EmployeeDB db, Employee employee) =>
{
    await db.Employees.AddAsync(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/employees/{employee.Id}", employee);
});

app.Run();
