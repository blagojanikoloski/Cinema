
using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using CinemaApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Controllers
{
    public class MyOrdersController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;
        private readonly IUserOrderService _userOrderService;

        public MyOrdersController(ApplicationDbContext context, UserManager<CinemaAppApplicationUser> userManager, IUserOrderService userOrderService)
        {
            _context = context;
            _userManager = userManager;
            _userOrderService = userOrderService;
        }

        [Authorize]
        [HttpGet]
        [Route("/MyOrders/")]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            // Retrieve UserOrders for the given UserID
            List<UserOrder> userOrders = _userOrderService.GetUserOrdersByUserId(userId);

            return View(userOrders);
        }

    }
}
