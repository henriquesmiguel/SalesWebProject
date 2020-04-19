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
    public class SellerService
    {
        private readonly SalesContext _context;

        public SellerService(SalesContext context)
        {
            _context = context;
        }
        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            Seller result = await _context.Seller.FirstOrDefaultAsync(s=> s.Id == id);
            if (result != null)
                result.Department = await _context.Department.FirstOrDefaultAsync(d => d.Id == result.DepartmentId);
            return result;
        }

        public async Task InsertAsync(Seller s)
        {
            _context.Add(s);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                Seller s = await FindByIdAsync(id);
                _context.Remove(s);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller s)
        {
            bool hasAny = await _context.Seller.AnyAsync(sl => sl.Id == s.Id);
            if (!hasAny)
                throw new NotFoundException("Seller Id not found");
            try
            {
                _context.Update(s);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }
}
