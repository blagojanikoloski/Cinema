using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExcelDataReader;
using System.IO;

namespace CinemaApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CinemaAppApplicationUser> userManager;
        private readonly SignInManager<CinemaAppApplicationUser> signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<CinemaAppApplicationUser> userManager, SignInManager<CinemaAppApplicationUser> signInManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this._dbContext = dbContext;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {

                if (!await _roleManager.RoleExistsAsync("standard"))
                {
                    var standardRole = new IdentityRole("standard");
                    await _roleManager.CreateAsync(standardRole);
                }

                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new CinemaAppApplicationUser
                    {
                        FirstName = request.Name,
                        LastName = request.LastName,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = request.PhoneNumber,

                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {

                        // Add the user to the "standard" role
                        await userManager.AddToRoleAsync(user, "standard");

                        var cart = new Cart { AppUserID = user.Id };
                        _dbContext.Cart.Add(cart);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }




        [HttpPost]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var userList = new List<UserExcelData>();

                using (var stream = file.OpenReadStream())
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Read the Excel file and extract user data
                        while (reader.Read())
                        {
                            var userData = new UserExcelData
                            {
                                Name = reader.GetString(0), // Assuming the name is in the first column
                                LastName = reader.GetString(1), // Assuming the last name is in the second column
                                Email = reader.GetString(2), // Assuming the email is in the third column
                                PhoneNumber = reader.GetString(3), // Assuming the phone number is in the fourth column
                                Password = reader.GetString(4), // Assuming the password is in the fifth column
                                Role = reader.GetString(5) // Assuming the password is in the fifth column
                            };

                            userList.Add(userData);
                        }
                    }
                }

                // Iterate through the extracted user data and register each user
                foreach (var userData in userList)
                {

                    var userExists = await userManager.FindByEmailAsync(userData.Email);
                    if (userExists != null)
                    {
                        // User with the same email already exists, skip to the next user
                        continue;
                    }

                    var registrationDto = new UserRegistrationDto
                    {
                        Name = userData.Name,
                        LastName = userData.LastName,
                        Email = userData.Email,
                        PhoneNumber = userData.PhoneNumber,
                        Password = userData.Password
                    };


                    // Pass the registration DTO to the Register method of the AccountController
                    var result = await Register(registrationDto);

                    if (userData.Role == "admin")
                    {
                        var user = await userManager.FindByEmailAsync(userData.Email);

                        if (user != null)
                        {
                            var currentRoles = await userManager.GetRolesAsync(user);
                            await userManager.RemoveFromRolesAsync(user, currentRoles);

                            // Add the user to the "admin" role
                            await userManager.AddToRoleAsync(user, "admin");
                        }
                    }
                    
                }

                // Return a success message or appropriate response
                return Ok("Users imported successfully");
            }

            // Handle the case when no file is uploaded
            ModelState.AddModelError("file", "Please upload a file");
            return View();
        }



       


    }
}

