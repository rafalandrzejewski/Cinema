using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Numery miejsc:")]
        public string SeatNumbers { get; set; }
        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Kod rezerwacji")]
        public string Code { get; set; }

    }
}
