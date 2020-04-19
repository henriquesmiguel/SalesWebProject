using Sales.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
