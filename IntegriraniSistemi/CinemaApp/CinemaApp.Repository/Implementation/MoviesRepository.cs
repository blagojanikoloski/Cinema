using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Repository.Implementation
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly ApplicationDbContext _context;

        public MoviesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movie.FirstOrDefault(m => m.MovieID == id);
        }

        public async Task<List<Movie>> GetLatestMoviesAsync(int count)
        {
            return await _context.Movie
                .OrderByDescending(m => m.MovieReleaseDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movie.ToListAsync();
        }



        // Implement other methods from IMovieRepository
    }
}