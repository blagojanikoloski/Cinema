namespace CinemaApp.Domain.DomainModels
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int MovieID { get; set; }
        public int Quantity { get; set; }
        public int MovieDatesID { get; set; }

        public CartItem()
        {

        }
    }
}
