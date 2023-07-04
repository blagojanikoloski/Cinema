using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using CinemaApp.Repository;
using CinemaApp.Domain.DomainModels;

namespace CinemaApp.Controllers
{
    public class MovieDatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieDatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("MovieDates/ShowTimes")]
        public IActionResult ShowTimes(int movieID)
        {
            var movieDates = _context.MovieDates.Where(md => md.MovieID == movieID).ToList();
            ViewBag.MovieID = movieID;
            return View(movieDates);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int movieID, MovieDates movieDate)
        {
            if (ModelState.IsValid)
            {
                movieDate.MovieID = movieID;
                _context.MovieDates.Add(movieDate);
                _context.SaveChanges();
                return RedirectToAction("ShowTimes", new { movieID = movieID });
            }
            return View(movieDate);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int movieID)
        {
            var movieDate = _context.MovieDates.Find(id);
            if (movieDate != null)
            {
                _context.MovieDates.Remove(movieDate);
                _context.SaveChanges();
            }
            return RedirectToAction("ShowTimes", new { movieID = movieID });
        }




    }
}
