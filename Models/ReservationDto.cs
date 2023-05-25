using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class ReservationDto
    {
        public string SeatNumbers { get; set; }
        public int? SeanceId { get; set; }
    }
}
