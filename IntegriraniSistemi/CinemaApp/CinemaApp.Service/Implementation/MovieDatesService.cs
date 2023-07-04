using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace CinemaApp.Service.Implementation
{
    public class MovieDatesService : IMovieDatesService
    {
        private readonly IMovieDatesRepository _movieDatesRepository;
        private readonly IMoviesRepository _moviesRepository;

        public MovieDatesService(IMovieDatesRepository movieDatesRepository, IMoviesRepository moviesRepository)
        {
            _movieDatesRepository = movieDatesRepository;
            _moviesRepository = moviesRepository;
        }

        public List<MovieDates> GetMovieDatesByMovieId(int movieId)
        {
            return _movieDatesRepository.GetMovieDatesByMovieId(movieId);
        }

        public int GetMovieDatesPriceById(int movieDatesID)
        {
            return _movieDatesRepository.GetMovieDatesPriceById(movieDatesID);
        }

        public MovieDates GetMovieDatesByID(int movieDatesID)
        {
            return _movieDatesRepository.GetMovieDatesByID(movieDatesID);
        }

        public async Task<IEnumerable<AdminViewTicketsDto>> GetMovieDatesWithDetailsAsync(string movieName, string genre)
        {
            var movieDates = await _movieDatesRepository.GetAllMovieDatesAsync();
            var movies = await _moviesRepository.GetAllMoviesAsync();

            var movieDatesWithDetails = from movieDate in movieDates
                                        join movie in movies on movieDate.MovieID equals movie.MovieID
                                        select new AdminViewTicketsDto
                                        {
                                            Date = movieDate.Date,
                                            Price = movieDate.Price,
                                            MovieName = movie.MovieName,
                                            Genre = movie.Genre
                                        };

            if (!string.IsNullOrEmpty(movieName))
            {
                movieDatesWithDetails = movieDatesWithDetails.Where(m => m.MovieName.Contains(movieName));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                movieDatesWithDetails = movieDatesWithDetails.Where(m => m.Genre.Contains(genre));
            }

            return movieDatesWithDetails;
        }
    }
}
