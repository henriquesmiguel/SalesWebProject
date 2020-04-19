using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.Models;
using Sales.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Sales.Services
{
    public class DepartmentService
    {
        private readonly SalesContext _context;

        public DepartmentService(SalesContext context)
        {
            _context = context;
        }
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(d=> d.Name).ToListAsync();
        }

        public Department FindById(int idTemp)
        {
            return _context.Department.Where(d => d.Id == idTemp).First();
        }

    }
}
