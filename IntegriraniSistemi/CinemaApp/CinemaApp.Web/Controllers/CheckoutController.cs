
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using CinemaApp.Repository;
using System.Threading.Tasks;
using System.Linq;
using CinemaApp.Domain.DomainModels;
using System.Collections.Generic;
using System;
using CinemaApp.Domain.Identity;
using CinemaApp.Service.Interface;

namespace CinemaApp.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;
        private readonly ICartsService _cartsService;
        private readonly ICartItemsService _cartItemsService;
        private readonly IUserOrderService _userOrderService;

        public CheckoutController(ApplicationDbContext context, UserManager<CinemaAppApplicationUser> userManager,ICartsService cartsService, ICartItemsService cartItemsService, IUserOrderService userOrderService)
        {
            _context = context;
            _userManager = userManager;
            _cartsService = cartsService;
            _cartItemsService = cartItemsService;
            _userOrderService = userOrderService;
        }



        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            int cartId = await _cartsService.GetCartIdByUserId(userId);

            List<CartItem> cartItems = _cartItemsService.GetCartItemsByCartId(cartId);


            int cartTotal=_cartsService.CalculateCartTotal(cartId);

            ViewBag.CartTotal = cartTotal;

            return View();

        }




        [HttpPost]
        public async Task<ActionResult> ProcessCheckoutAsync(string nameOnCard, string cardNumber, string expireDate, string cvv, decimal cartTotal)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            int cartId = await _cartsService.GetCartIdByUserId(userId);

            DateTime date = DateTime.Now;

            List<CartItem> cartItems = _cartItemsService.GetCartItemsByCartId(cartId);

            _userOrderService.AddUserOrder(userId, date, cartTotal, cartItems);


            await _context.SaveChangesAsync();

            _cartItemsService.DeleteCartItemsByCartID(cartId);

            await _context.SaveChangesAsync();
            //TILL HERE I CREATE USERORDER AND EACH ORDERITEMS FOR THAT ORDER

            // Return a response message or redirect to another page
            return Redirect("/Checkout/Success");
        }

        public async Task<IActionResult> SuccessAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            int orderId = _userOrderService.GetLatestOrderID(userId);
            ViewBag.OrderID = orderId;

            return View();
        }
    }
}
