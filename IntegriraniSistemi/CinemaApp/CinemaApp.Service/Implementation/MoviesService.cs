using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Implementation
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly IMovieDatesRepository _movieDatesRepository;

        public MoviesService(IMoviesRepository moviesRepository, IMovieDatesRepository movieDatesRepository)
        {
            _moviesRepository = moviesRepository;
            _movieDatesRepository = movieDatesRepository;
        }

        public Movie GetMovieById(int id)
        {
            return _moviesRepository.GetMovieById(id);
        }


        public async Task<List<Movie>> GetLatestMoviesAsync(int count)
        {
            return await _moviesRepository.GetLatestMoviesAsync(count);

        }

        public async Task<List<Movie>> GetMoviesByDateAsync(DateTime searchDate)
        {
            var movieDates = await _movieDatesRepository.GetAllMovieDatesAsync();

            var movieIds = movieDates
                .Where(md => md.Date.Date == searchDate.Date)
                .Select(md => md.MovieID)
                .ToList();

            var movies = await _moviesRepository.GetAllMoviesAsync();
            var filteredMovies = movies.Where(m => movieIds.Contains(m.MovieID)).ToList();

            return filteredMovies;
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _moviesRepository.GetAllMoviesAsync();
        }
        // Implement other methods from IMovieService
    }
}
