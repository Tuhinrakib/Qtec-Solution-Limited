using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeVM
    {
        public EmployeeVM()
        {
            this.EmployeeList = new List<int>();
        }
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

        // Foreign Key for Department
        public int DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public List<int> EmployeeList { get; set; }
    }
}
