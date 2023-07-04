
using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using CinemaApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace CinemaApp.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserOrderService _orderService;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;

        public AdminController(IUserOrderService orderService, UserManager<CinemaAppApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }



        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;
            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                if (userCheck == null)
                {
                    var user = new CinemaAppApplicationUser
                    {
                        FirstName = item.Name,
                        LastName = item.LastName,
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = item.PhoneNumber,
                        UserCart = new ShoppingCart()
                    };
                    var result = _userManager.CreateAsync(user, item.Password).Result;

                    status = status & result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return status;
        }
    }
}
