using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    // 15.11.2025
    // SeedData klass andmete genereerimiseks
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        // Generate meetod koordineerib andmete genereerimist
        public void Generate()
        {
            // Ära tee midagi kui andmed on juba olemas
            if (_dbContext.Employees.Any())
            {
                return;
            }

            GenerateEmployees();
            _dbContext.SaveChanges();

            GenerateProjects();
            _dbContext.SaveChanges();

            GenerateTasks();
            _dbContext.SaveChanges();

            GenerateProjectMembers();
            _dbContext.SaveChanges();
        }

        private void GenerateEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Jaan",
                    LastName = "Tamm",
                    Email = "jaan.tamm@company.com",
                    Phone = "5551234567",
                    Role = "Developer"
                },
                new Employee
                {
                    FirstName = "Maria",
                    LastName = "Kask",
                    Email = "maria.kask@company.com",
                    Phone = "5559876543",
                    Role = "Manager"
                },
                new Employee
                {
                    FirstName = "Peeter",
                    LastName = "Ots",
                    Email = "peeter.ots@company.com",
                    Phone = "5555555555",
                    Role = "Developer"
                },
                new Employee
                {
                    FirstName = "Katrin",
                    LastName = "Lepp",
                    Email = "katrin.lepp@company.com",
                    Phone = "5554444444",
                    Role = "QA"
                },
                new Employee
                {
                    FirstName = "Raivo",
                    LastName = "Saar",
                    Email = "raivo.saar@company.com",
                    Phone = "5553333333",
                    Role = "DevOps"
                }
            };

            _dbContext.Employees.AddRange(employees);
        }

        private void GenerateProjects()
        {
            var now = DateTime.Now;
            var projects = new List<Project>
            {
                new Project
                {
                    Name = "Web Application Redesign",
                    Description = "Modernize the company website with new UI/UX",
                    StartDate = now,
                    EndDate = now.AddMonths(3),
                    Status = "Active",
                    Budget = 50000m
                },
                new Project
                {
                    Name = "Mobile App Development",
                    Description = "Develop a new mobile application for iOS and Android",
                    StartDate = now.AddDays(7),
                    EndDate = now.AddMonths(6),
                    Status = "Planned",
                    Budget = 100000m
                },
                new Project
                {
                    Name = "Database Migration",
                    Description = "Migrate legacy database to modern cloud solution",
                    StartDate = now.AddDays(14),
                    EndDate = now.AddMonths(2),
                    Status = "Active",
                    Budget = 30000m
                }
            };

            _dbContext.Projects.AddRange(projects);
        }

        private void GenerateTasks()
        {
            var now = DateTime.Now;
            var projects = _dbContext.Projects.ToList();
            var employees = _dbContext.Employees.ToList();

            if (projects.Count >= 3 && employees.Count >= 5)
            {
                var tasks = new List<Data.Task>
                {
                    new Data.Task
                    {
                        Title = "Design UI mockups",
                        Description = "Create high-fidelity mockups for the new website design",
                        ProjectId = projects[0].Id,
                        AssignedTo = employees[1].Id,
                        Status = "In Progress",
                        StartDate = now,
                        EndDate = now.AddDays(5),
                        Priority = "High"
                    },
                    new Data.Task
                    {
                        Title = "Frontend development",
                        Description = "Implement the UI components in React",
                        ProjectId = projects[0].Id,
                        AssignedTo = employees[0].Id,
                        Status = "Pending",
                        StartDate = now.AddDays(5),
                        EndDate = now.AddDays(15),
                        Priority = "High"
                    },
                    new Data.Task
                    {
                        Title = "Backend API integration",
                        Description = "Connect frontend with existing backend services",
                        ProjectId = projects[0].Id,
                        AssignedTo = employees[2].Id,
                        Status = "Pending",
                        StartDate = now.AddDays(10),
                        EndDate = now.AddDays(20),
                        Priority = "High"
                    },
                    new Data.Task
                    {
                        Title = "Requirements gathering",
                        Description = "Collect and document mobile app requirements",
                        ProjectId = projects[1].Id,
                        AssignedTo = employees[1].Id,
                        Status = "In Progress",
                        StartDate = now.AddDays(7),
                        EndDate = now.AddDays(12),
                        Priority = "Medium"
                    },
                    new Data.Task
                    {
                        Title = "Database schema design",
                        Description = "Design the new database schema for migration",
                        ProjectId = projects[2].Id,
                        AssignedTo = employees[4].Id,
                        Status = "In Progress",
                        StartDate = now.AddDays(14),
                        EndDate = now.AddDays(21),
                        Priority = "Critical"
                    },
                    new Data.Task
                    {
                        Title = "QA testing",
                        Description = "Test the redesigned website across all browsers",
                        ProjectId = projects[0].Id,
                        AssignedTo = employees[3].Id,
                        Status = "Pending",
                        StartDate = now.AddDays(20),
                        EndDate = now.AddDays(28),
                        Priority = "Medium"
                    }
                };

                _dbContext.Tasks.AddRange(tasks);
            }
        }

        private void GenerateProjectMembers()
        {
            // Get actual IDs from database after employees and projects are saved
            var employees = _dbContext.Employees.ToList();
            var projects = _dbContext.Projects.ToList();

            if (employees.Count >= 5 && projects.Count >= 3)
            {
                var projectMembers = new List<ProjectMember>
                {
                    new ProjectMember { ProjectId = projects[0].Id, EmployeeId = employees[0].Id, RoleInProject = "Developer" },
                    new ProjectMember { ProjectId = projects[0].Id, EmployeeId = employees[1].Id, RoleInProject = "Manager" },
                    new ProjectMember { ProjectId = projects[0].Id, EmployeeId = employees[3].Id, RoleInProject = "QA" },
                    new ProjectMember { ProjectId = projects[1].Id, EmployeeId = employees[0].Id, RoleInProject = "Lead Developer" },
                    new ProjectMember { ProjectId = projects[1].Id, EmployeeId = employees[1].Id, RoleInProject = "Manager" },
                    new ProjectMember { ProjectId = projects[1].Id, EmployeeId = employees[2].Id, RoleInProject = "Developer" },
                    new ProjectMember { ProjectId = projects[2].Id, EmployeeId = employees[4].Id, RoleInProject = "DevOps" },
                    new ProjectMember { ProjectId = projects[2].Id, EmployeeId = employees[1].Id, RoleInProject = "Manager" }
                };

                _dbContext.ProjectMembers.AddRange(projectMembers);
            }
        }
    }
}