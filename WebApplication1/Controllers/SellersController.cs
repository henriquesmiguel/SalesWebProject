using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.Models.Entities;
using Sales.Models.ViewModels;
using Sales.Services;
using Sales.Services.Exceptions;

namespace Sales.Controllers
{
    public class SellersController : Controller
    {
        private readonly SalesContext _context;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SalesContext context, SellerService sellerService, DepartmentService departmentService)
        {
            _context = context;
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        // GET: Sellers
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
           
            return View(list);
            //return View(await _context.Seller.ToListAsync());
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            Seller seller = await _sellerService.FindByIdAsync(id ?? default(int));

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });
            }

            return View(seller);
        }

        // GET: Sellers/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,BirthDate,BaseSalary")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                await _sellerService.InsertAsync(seller);
                /*
                _context.Add(seller);
                await _context.SaveChangesAsync();
                */
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            Seller seller = await _sellerService.FindByIdAsync(id ?? default(int));

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });
            }

            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };
            return View(viewModel);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            if (ModelState.IsValid)
            {
                try
                {           
                    await _sellerService.UpdateAsync(seller);
                    //await _context.SaveChangesAsync();
                }
                catch (NotFoundException e)
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
                catch (DbConcurrencyException e)
                {
                    if (!SellerExists(seller.Id))
                    {
                        return RedirectToAction(nameof(Error), new { message = "Id not found" });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Error), new { message = e.Message });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            Seller seller = await _sellerService.FindByIdAsync(id ?? default(int));

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller not found" });
            }

            //var departments = _departmentService.FindAll();
            //var viewModel = new SellerFormViewModel { Departments = departments };

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }

        public IActionResult Error (string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(viewModel);
        }
    }
}
