using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CinemaApp.Domain.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Repository;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using System.Collections.Generic;
using CinemaApp.Domain.DTO;
using ClosedXML.Excel;
using System.IO;
using CinemaApp.Service.Interface;
using System.Net.Http;

namespace CinemaApp.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMovieDatesService _movieDatesService;

        public AdminController(UserManager<CinemaAppApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context,IMovieDatesService movieDatesService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _movieDatesService = movieDatesService;
        }

        public IActionResult Index()
        {

            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Users(string email)
        {

            var users = _userManager.Users.ToList();

            if (!string.IsNullOrEmpty(email))
            {
                // Perform filtering logic based on the provided email
                users = users.Where(u => u.Email.Contains(email)).ToList();
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Tickets(string movieName, string genre)
        {
            var movieDates = _context.MovieDates.ToList();
            var movies = _context.Movie.ToList();

            var movieDatesWithDetails = await _movieDatesService.GetMovieDatesWithDetailsAsync(movieName, genre);

            return View(movieDatesWithDetails);
        }

        [HttpGet]
        public FileContentResult ExportToExcel(string movieName, string genre)
        {



            var filteredTickets = _movieDatesService.GetMovieDatesWithDetailsAsync(movieName, genre).Result;
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("Tickets");

                worksheet.Cell(1, 1).Value = "Date";
                worksheet.Cell(1, 2).Value = "Price";
                worksheet.Cell(1, 3).Value = "Movie Name";
                worksheet.Cell(1, 4).Value = "Genre";

                int row = 2;
                foreach (var ticket in filteredTickets)
                {
                    worksheet.Cell(row, 1).Value = ticket.Date;
                    worksheet.Cell(row, 2).Value = ticket.Price;
                    worksheet.Cell(row, 3).Value = ticket.MovieName;
                    worksheet.Cell(row, 4).Value = ticket.Genre;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }

        }






        [HttpPost]
        public async Task<IActionResult> SetAsAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return NotFound();
            }

            // Check if the "admin" role exists, if not, create it
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole("admin");
                await _roleManager.CreateAsync(adminRole);
            }

            // Remove the user from any existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add the user to the "admin" role
            await _userManager.AddToRoleAsync(user, "admin");

            // Redirect back to the Index action
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> SetAsStandard(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return NotFound();
            }

            // Check if the "standard" role exists, if not, create it
            if (!await _roleManager.RoleExistsAsync("standard"))
            {
                var standardRole = new IdentityRole("standard");
                await _roleManager.CreateAsync(standardRole);
            }

            // Remove the user from any existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add the user to the "standard" role
            await _userManager.AddToRoleAsync(user, "standard");

            // Check if the user is the currently logged-in user
            var loggedInUserId = _userManager.GetUserId(User);
            if (user.Id == loggedInUserId)
            {
                // Log out the user and redirect to the Logout action
                return Redirect("/Account/Logout");
            }

            // Redirect back to the Index action
            return RedirectToAction("Users");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    // User deleted successfully
                    return RedirectToAction("Users");
                }
                else
                {
                    // Failed to delete the user, handle the error accordingly
                    // You can display an error message or redirect to an error page
                    // For simplicity, I'm returning a general error message
                    return RedirectToAction("Users", new { error = "Failed to delete the user." });
                }
            }

            // User not found, handle the error accordingly
            // You can display an error message or redirect to an error page
            // For simplicity, I'm returning a general error message
            return RedirectToAction("Users", new { error = "User not found." });
        }

    }
}