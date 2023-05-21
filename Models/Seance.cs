using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models
{
    public class Seance
    {
        public int Id { get; set; }
        [Display(Name = "Data:")]
        public DateTime Date { get; set; }
        [Display(Name = "Maksymalna ilość miejsc:")]
        public int MaxSeatCount { get; set; }
        public int FreeSeatCount { get; set;}
        public string SeatsJsonObject { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
