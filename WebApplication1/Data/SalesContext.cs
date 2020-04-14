using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sales.Models.Entities;

namespace Sales.Models
{
    public class SalesContext : DbContext
    {
        public SalesContext (DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        public DbSet<Entities.Department> Department { get; set; }

        public DbSet<Entities.Seller> Seller { get; set; }

        public DbSet<Entities.SalesRecord> SalesRecord { get; set; }
    }
}
