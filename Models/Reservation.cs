using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Reservation
    {
        
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SeanceId { get; set; }
        public Seance? Seance { get; set; }
        public int UserId { get; set; }
        public IdentityUser? User { get; set; }
        public List<SeatNumber>? SeatNumbers { get; set; }

        public class SeatNumber
        {
            public int Id { get; set; }
            public string? Number { get; set; }
            public int ReservationId { get; set; }
            public Reservation? Reservation { get; set; }
        }

    }
}
