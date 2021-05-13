using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiverMan.DataAccessLayer;
using RiverMan.Models;

namespace RiverMan.Controllers
{
    public class SubscriptionServiceController : Controller
    {
        private readonly RiverManContext _context;

        public SubscriptionServiceController(RiverManContext context)
        {
            _context = context;
        }

        // GET: SubscriptionService
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubscriptionServices.ToListAsync());
        }

        // GET: SubscriptionService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionService = await _context.SubscriptionServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionService == null)
            {
                return NotFound();
            }

            return View(subscriptionService);
        }

        // GET: SubscriptionService/Create
        public IActionResult Create()
        {
            var serviceTypes = _context.ServiceTypes.ToList();
            ViewData["ServiceTypes"] = new SelectList(serviceTypes, "Id", "Name");
            return View();
        }

        // POST: SubscriptionService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceName,ServiceTypeId,ImageURI")] SubscriptionService subscriptionService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscriptionService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscriptionService);
        }

        // GET: SubscriptionService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionService = await _context.SubscriptionServices.FindAsync(id);
            if (subscriptionService == null)
            {
                return NotFound();
            }
            return View(subscriptionService);
        }

        // POST: SubscriptionService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceName,ImageURI")] SubscriptionService subscriptionService)
        {
            if (id != subscriptionService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionServiceExists(subscriptionService.Id))
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
            return View(subscriptionService);
        }

        // GET: SubscriptionService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionService = await _context.SubscriptionServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionService == null)
            {
                return NotFound();
            }

            return View(subscriptionService);
        }

        // POST: SubscriptionService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscriptionService = await _context.SubscriptionServices.FindAsync(id);
            _context.SubscriptionServices.Remove(subscriptionService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionServiceExists(int id)
        {
            return _context.SubscriptionServices.Any(e => e.Id == id);
        }
    }
}
