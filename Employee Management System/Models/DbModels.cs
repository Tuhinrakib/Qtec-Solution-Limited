using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Position { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        public string Status { get; set; } = "active";

        public bool IsDeleted { get; set; }

        // Foreign Key for Department
        public int DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
    }
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required]
        public string? DepartmentName { get; set; }

        public bool IsDeleted { get; set; }

        public int? ManagerID { get; set; }
        public decimal Budget { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
    public class PerformanceReview
    {
        [Key] 
        public int ReviewID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        [Range(1, 10)]
        public int ReviewScore { get; set; }
        public bool IsDeleted { get; set; }
        public string ReviewNotes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerformanceReview>()
                .HasKey(pr => pr.ReviewID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
