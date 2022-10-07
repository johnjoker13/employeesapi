using Microsoft.EntityFrameworkCore;
namespace EmployeesAPI.Models

{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Undername { get; set; }
        public string? Role { get; set; }
    }

    class EmployeeDB : DbContext
    {
        public EmployeeDB(DbContextOptions options) : base(options) { }
        public DbSet<Employee> Employees { get; set; } = null!;
    }

}
