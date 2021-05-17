using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiverMan.DataAccessLayer;
using RiverMan.Models;
using System.Security.Claims;

namespace RiverMan.Controllers
{
    public class UserSubscriptionsController : Controller
    {
        private readonly RiverManContext _context;

        public UserSubscriptionsController(RiverManContext context)
        {
            _context = context;
        }

        // GET: UserSubscriptions
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var riverManContext = _context.UserSubscriptions.Include(u => u.Subscription).Include(u => u.User).Where(u => u.UserId == user);
            return View(await riverManContext.ToListAsync());
        }

        // GET: UserSubscriptions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSubscription = await _context.UserSubscriptions
                .Include(u => u.Subscription)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userSubscription == null)
            {
                return NotFound();
            }

            return View(userSubscription);
        }

        // GET: UserSubscriptions/Create
        public IActionResult Create()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userSubscriptions = _context.UserSubscriptions.Where(c => c.UserId == user).Select(c=>c.SubscriptionId).ToList();
            var userUnsubscribed = _context.SubscriptionServices.Where(c => !userSubscriptions.Contains(c.Id)).ToList();
            ViewData["Subscription"] = new SelectList(userUnsubscribed, "Id", "ServiceName");
            return View();
        }

        // POST: UserSubscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,SubscriptionId")] UserSubscription userSubscription)
        {
            if (ModelState.IsValid)
            {
                userSubscription.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(userSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubscriptionId"] = new SelectList(_context.SubscriptionServices, "Id", "Id", userSubscription.SubscriptionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubscription.UserId);
            return View(userSubscription);
        }

        // GET: UserSubscriptions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSubscription = await _context.UserSubscriptions.FindAsync(id);
            if (userSubscription == null)
            {
                return NotFound();
            }
            ViewData["SubscriptionId"] = new SelectList(_context.SubscriptionServices, "Id", "Id", userSubscription.SubscriptionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubscription.UserId);
            return View(userSubscription);
        }

        // POST: UserSubscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,SubscriptionId")] UserSubscription userSubscription)
        {
            if (id != userSubscription.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSubscriptionExists(userSubscription.UserId))
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
            ViewData["SubscriptionId"] = new SelectList(_context.SubscriptionServices, "Id", "Id", userSubscription.SubscriptionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userSubscription.UserId);
            return View(userSubscription);
        }

        // GET: UserSubscriptions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSubscription = await _context.UserSubscriptions
                .Include(u => u.Subscription)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userSubscription == null)
            {
                return NotFound();
            }

            return View(userSubscription);
        }

        // POST: UserSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userSubscription = await _context.UserSubscriptions.FindAsync(id);
            _context.UserSubscriptions.Remove(userSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSubscriptionExists(string id)
        {
            return _context.UserSubscriptions.Any(e => e.UserId == id);
        }
    }
}
