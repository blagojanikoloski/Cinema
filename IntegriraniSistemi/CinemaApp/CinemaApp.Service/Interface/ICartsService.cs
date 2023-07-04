using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Interface
{
    public interface ICartsService
    {
        Task<int> GetCartIdByUserId(string userId);
        int CalculateCartTotal(int cartId);


    }
}
