using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CinemaApp.Repository;
using Microsoft.Extensions.Logging;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Service.Interface;

namespace CinemaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly IMoviesService _moviesService;
        private readonly IMovieDatesService _movieDatesService;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMoviesService moviesService, IMovieDatesService movieDatesService)
        {
            _logger = logger;
            _context = context;
            _moviesService = moviesService;
            _movieDatesService = movieDatesService;
        }

        public async Task<IActionResult> Index()
        {

            List<Movie> movies = await _moviesService.GetLatestMoviesAsync(3);

            return View(movies);

 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}