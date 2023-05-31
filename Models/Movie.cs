using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [Display(Name = "Tytuł filmu:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany.")]
        [Display(Name = "Opis filmu:")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Czas trwania filmu jest wymagany.")]
        [Display(Name = "Czas trwania filmu (minuty):")]
        [Range(1, 500)]
        public int Time { get; set;}
    }
}
