using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Interface
{
    
    public interface IMoviesService
    {
        Movie GetMovieById(int id);

        Task<List<Movie>> GetLatestMoviesAsync(int count);

        Task<List<Movie>> GetMoviesByDateAsync(DateTime searchDate);

        Task<List<Movie>> GetAllMoviesAsync();

    }
}

