using System;

namespace CinemaApp.Domain.DomainModels
{
    public class UserOrder
    {
        public int UserOrderID { get; set; }
        public string AppUserID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }

        public UserOrder()
        {

        }
    }
}
