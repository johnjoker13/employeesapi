using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EmployeesAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("EmployeesLocalAccess");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EmployeeDB>((options) => options.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo {
    Title = "Employees API",
    Description = "Employees List",
    Version = "v1" 
  });
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
app.MapGet("/employee/{id}", async (EmployeeDB db, int id) => await db.Employees.FindAsync(id));
app.MapPut("/employee/{id}", async (EmployeeDB db, Employee updateEmployee, int id) =>
{
   var employee = await db.Employees.FindAsync(id);
   if (employee is null) return Results.NotFound();
   employee.Name = updateEmployee.Name;
   employee.Undername = updateEmployee.Undername;
   employee.Role = updateEmployee.Role;
   await db.SaveChangesAsync();
   return Results.NoContent();
});
app.MapDelete("/employee/{id}", async (EmployeeDB db, int id) =>
{
   var employee = await db.Employees.FindAsync(id);
   if (employee is null)
   {
     return Results.NotFound();
   }
   db.Employees.Remove(employee);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
