using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Implementation
{
    public class MovieDatesRepository : IMovieDatesRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieDatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MovieDates> GetMovieDatesByMovieId(int movieId)
        {
            return _context.MovieDates
                .Where(md => md.MovieID == movieId)
                .ToList();
        }

        public async Task<int?> GetPriceByMovieDatesIDAsync(int movieDatesID)
        {
            return await _context.MovieDates
                .Where(md => md.MovieDatesID == movieDatesID)
                .Select(md => md.Price)
                .FirstOrDefaultAsync();
        }


        public int GetMovieDatesPriceById(int movieDatesID)
        {
            return _context.MovieDates
                .Where(md => md.MovieDatesID == movieDatesID)
                .Select(md => md.Price)
                .FirstOrDefault();
        }

        public MovieDates GetMovieDatesByID(int movieDatesID)
        {
            return _context.MovieDates.FirstOrDefault(md => md.MovieDatesID == movieDatesID);
        }

        public async Task<List<MovieDates>> GetAllMovieDatesAsync()
        {
            return await _context.MovieDates.ToListAsync();
        }
    }
}
