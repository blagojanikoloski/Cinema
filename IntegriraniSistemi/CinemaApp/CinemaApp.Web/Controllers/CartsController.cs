using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Repository;
using CinemaApp.Domain.Identity;
using Stripe;
using CinemaApp.Domain.DomainModels;
using System.Security.Claims;
using CinemaApp.Service.Interface;

namespace CinemaApp.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;
        private readonly ICartsService _cartsService;
        private readonly ICartItemsService _cartItemsService;

        public CartsController(ApplicationDbContext context, UserManager<CinemaAppApplicationUser> userManager, ICartsService cartsService, ICartItemsService cartItemsService)
        {
            _context = context;
            _userManager = userManager;
            _cartsService = cartsService;
            _cartItemsService = cartItemsService;
        }
        [Authorize]
        [HttpGet]
        [Route("/Carts/")]
        public async Task<IActionResult> Index(string userId)
        {

                int cartId = await _cartsService.GetCartIdByUserId(userId);

                List<(int CartItemID,string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> cartItemDetails = _cartItemsService.GetCartItemDetails(cartId);

                ViewBag.CartItems = cartItemDetails;

                return View(cartItemDetails);
            
        }






        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdateAsync(int cartItemID, string action)
        {
            // Find the cart item in the database based on the cartItemId
            CartItem cartItem = await _cartItemsService.GetCartItemAsync(cartItemID);

            _cartItemsService.UpdateCartItemQuantity(cartItem, action);

            // Save the changes to the database
            await _context.SaveChangesAsync();


            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;
            // Get the current URL

            var redirectUrl = $"{Request.Scheme}://{Request.Host.Value}/Carts/?userId={userId}";
            // Redirect to the current page
            return Redirect(redirectUrl);
        }

        // GET: Carts/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,AppUserID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,AppUserID")] Cart cart)
        {
            if (id != cart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartID))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.CartID == id)).GetValueOrDefault();
        }


        

        
    }
}
