using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public List<string>? SeatNumberList { get; set; }
        public DateTime Date { get; set; }
        public int SeanceId { get; set; }
        public Seance? Seance { get; set; }
        public int UserId { get; set; }
        public IdentityUser? User { get; set; }

    }
}
