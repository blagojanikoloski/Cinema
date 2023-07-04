namespace CinemaApp.Domain.DomainModels
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int UserOrderID { get; set; }
        public int MovieDatesID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public OrderItem()
        {

        }
    }
}
