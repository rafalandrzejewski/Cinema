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
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string SeatNumbers { get; set; }

    }
}
