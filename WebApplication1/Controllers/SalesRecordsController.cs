using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.Models.Entities;
using Sales.Services;


namespace Sales.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesContext _context;
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesContext context, SalesRecordService salesRecordService, SellerService sellerService)
        {
            _context = context;
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }

        // GET: SalesRecords
        public async Task<IActionResult> Index(int? sellerId)
        {
            Seller s = new Seller();
            if (sellerId != null)
            {
               s = await _sellerService.FindByIdAsync(sellerId.Value);
            }
            return View(s);
        }

        public async Task<IActionResult> SimpleSearch(DateTime minDate, DateTime maxDate, int? sellerId)
        {
            ViewData["minDate"] = minDate; //.ToString("yyyy-mm-dd");
            ViewData["maxDate"] = maxDate; //.ToString("yyyy-mm-dd");

            var list = await _salesRecordService.FindAllSimpleAsync(minDate, maxDate, sellerId);

            return View(list);
        }

        public async Task<IActionResult> GroupingSearch(DateTime minDate, DateTime maxDate, int? sellerId)
        {
            ViewData["minDate"] = minDate; //.ToString("yyyy-mm-dd");
            ViewData["maxDate"] = maxDate; //.ToString("yyyy-mm-dd");

            var list = await _salesRecordService.FindAllGroupedAsync(minDate, maxDate, sellerId);
            return View(list);
        }

        // GET: SalesRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // GET: SalesRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Amount,Status")] SalesRecord salesRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesRecord);
        }

        // GET: SalesRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord.FindAsync(id);
            if (salesRecord == null)
            {
                return NotFound();
            }
            return View(salesRecord);
        }

        // POST: SalesRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Amount,Status")] SalesRecord salesRecord)
        {
            if (id != salesRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesRecordExists(salesRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(salesRecord);
        }

        // GET: SalesRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // POST: SalesRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesRecord = await _context.SalesRecord.FindAsync(id);
            _context.SalesRecord.Remove(salesRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesRecordExists(int id)
        {
            return _context.SalesRecord.Any(e => e.Id == id);
        }
    }
}
