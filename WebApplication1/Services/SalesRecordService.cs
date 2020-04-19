using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.Models.Entities;
using Sales.Services.Exceptions;

namespace Sales.Services
{
    public class SalesRecordService
    {
        private readonly SalesContext _context;

        public SalesRecordService(SalesContext context)
        {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindAllSimpleAsync(DateTime dtMin, DateTime dtMax, int? sellerId)
        {
            bool existsSeller = await _context.Seller.AnyAsync(s => s.Id == sellerId);
            if (!existsSeller)
                return await _context.SalesRecord.Where(s => s.Date >= dtMin && s.Date < dtMax).OrderBy(s => s.Date).ToListAsync();
            else
                return await _context.SalesRecord.Where(s => s.Date >= dtMin && s.Date < dtMax && s.SellerID == sellerId).OrderBy(s => s.Date).ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindAllGroupedAsync(DateTime dtMin, DateTime dtMax, int? sellerId)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (sellerId.HasValue)
            {
                result = result.Where(x => x.Date >= dtMin && x.Date < dtMax && x.SellerID == sellerId);
            }
            else
            {
                result = result.Where(x => x.Date >= dtMin && x.Date < dtMax);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }

    }
}
