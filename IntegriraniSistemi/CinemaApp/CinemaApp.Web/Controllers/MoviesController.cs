using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;

namespace CinemaApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<CinemaAppApplicationUser> _userManager;

        private readonly IMoviesService _moviesService;

        private readonly IMovieDatesService _movieDatesService;

        private readonly ICartsService _cartsService;

        private readonly ICartItemsService _cartItemsService;
        public MoviesController(ApplicationDbContext context, UserManager<CinemaAppApplicationUser> userManager, IMoviesService moviesService, IMovieDatesService movieDatesService, ICartsService cartsService, ICartItemsService cartItemsService)
        {
            _context = context;
            _userManager = userManager;
            _moviesService = moviesService;
            _movieDatesService = movieDatesService;
            _cartsService = cartsService;
            _cartItemsService = cartItemsService;
        }

        // GET: Movies
        [Authorize]
        public async Task<IActionResult> Index(DateTime? searchDate)
        {
            List<Movie> movies;

            if (searchDate != null && searchDate.HasValue)
            {
                movies = await _moviesService.GetMoviesByDateAsync(searchDate.Value);
            }
            else
            {
                movies = await _moviesService.GetAllMoviesAsync();
            }

            return View(movies);
        }


        // GET: Movies/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            List<MovieDates> movieDates = _movieDatesService.GetMovieDatesByMovieId(id.Value);

            // Pass movieDates to the view using ViewBag
            ViewBag.MovieDates = movieDates;

            var movie = _moviesService.GetMovieById(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }


        // GET: Movies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieID,MovieName,MovieDescription,MovieReleaseDate,Genre")] Movie movie, List<MovieDates> movieDates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);

                // Save the movie first to generate its MovieID
                await _context.SaveChangesAsync();

                // Create individual MovieDates objects and associate them with the movie
                if (movieDates != null && movieDates.Count > 0)
                {
                    foreach (var date in movieDates)
                    {
                        date.MovieID = movie.MovieID; // Associate with the newly created movie
                        _context.Add(date);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }



        // GET: Movies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }


            dynamic mymodel = new ExpandoObject();

            var movieDates = await _context.MovieDates
                .Where(d => d.MovieID == id)
                .ToListAsync();

            mymodel.MovieDates = movieDates;
            mymodel.Movie = movie;
            ViewBag.Movie = movie; // Pass the movie object to the view using ViewBag
            ViewBag.MovieDates = movieDates; // Pass the list of movie dates to the view using ViewBag

            return View(movie);
        }


        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieID,MovieName,MovieDescription,MovieReleaseDate,Genre")] Movie movie)
        {
            if (id != movie.MovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieID))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.MovieID == id)).GetValueOrDefault();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCartAsync(int movieID, int movieDatesID, int quantity)
        {
            // Get the UserID of the currently logged-in user
            string userId = _userManager.GetUserId(User);

            // Query the database to find the CartID associated with the AppUserID
            int cartId = await _cartsService.GetCartIdByUserId(userId);

            // Create a new CartItem object
            _cartItemsService.AddCartItem(cartId, movieID, quantity, movieDatesID);

            // Redirect or return a response
            return RedirectToAction("Index", "Movies");
        }

    }
}
