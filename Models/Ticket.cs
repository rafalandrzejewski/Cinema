namespace Cinema.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public enum TicketType { Regular, Discounted }
        public TicketType Type { get; set; }
        public decimal Price
        {
            get
            {
                return this.Type == TicketType.Regular ? 25m : 18m;
            }
        }
    }
}
