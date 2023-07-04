using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Interface
{
    public interface IMoviesRepository
    {
        Movie GetMovieById(int id);

        Task<List<Movie>> GetLatestMoviesAsync(int count);

        Task<List<Movie>> GetAllMoviesAsync();

    }
}
