using Employee_Management_System.Helper;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string departmentFilter, string positionFilter, int? pageNumber)
        {
            ViewData["Departments"] = await _context.Departments.ToListAsync();
            ViewData["SearchString"] = searchString;
            ViewData["DepartmentFilter"] = departmentFilter;
            ViewData["PositionFilter"] = positionFilter;

            var employees = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(departmentFilter))
            {
                employees = employees.Where(e => e.Department.DepartmentName == departmentFilter);
            }

            if (!string.IsNullOrEmpty(positionFilter))
            {
                employees = employees.Where(e => e.Position == positionFilter);
            }

            int pageSize = 5;
            int currentPage = pageNumber ?? 1;
            var paginatedEmployees = await PaginatedList<Employee>.CreateAsync(employees, currentPage, pageSize);

            return View(paginatedEmployees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,Email,Phone,Position,JoinDate,Status,DepartmentID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Name,Email,Phone,Position,JoinDate,Status,DepartmentID")] Employee employee)
        {
            if (id != employee.EmployeeID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/SoftDelete
        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            // Mark the employee as deleted
            employee.IsDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee marked as deleted successfully." });
        }

        // POST: Employees/UndoDelete
        [HttpPost]
        public async Task<IActionResult> UndoDelete(int id)
        {
            var employee = await _context.Employees.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.EmployeeID == id);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            // Restore the employee
            employee.IsDeleted = false;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee restored successfully." });
        }

        // POST: Employees/PermanentDelete
        [HttpPost]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var employee = await _context.Employees.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.EmployeeID == id);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            // Permanently delete the employee
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee permanently deleted successfully." });
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}
