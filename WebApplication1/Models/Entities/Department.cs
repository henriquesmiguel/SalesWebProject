using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; }

        public Department()
        {
            Sellers = new List<Seller>();
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
            Sellers = new List<Seller>();
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime startDate, DateTime endDate)
        {
            double returnSum = 0.0;
            foreach (Seller seller in Sellers)
            {
                returnSum += seller.TotalSales(startDate, endDate);
            }

            return returnSum;
        }
    }
}
