using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sales.Models.Entities;

namespace Sales.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> departments = new List<Department>();
            departments.Add(new Department(1, "Electronics"));
            departments.Add(new Department(2, "Fashion"));
            departments.Add(new Department(3, "Toys"));        

            return View(departments);
        }

        public IActionResult Create()
        {
            /*
            List<Department> departments = new List<Department>();
            departments.Add(new Department(1, "Electronics"));
            departments.Add(new Department(2, "Fashion"));
            departments.Add(new Department(3, "Toys"));
            */
            return NotFound();
        }
    }
}