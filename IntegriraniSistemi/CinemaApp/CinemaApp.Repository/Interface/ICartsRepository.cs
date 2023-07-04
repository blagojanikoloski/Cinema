using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Interface
{
    public interface ICartsRepository
    {
        Task<int> GetCartIdByUserId(string userId);
    }
}
