using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł filmu:")]
        public string Title { get; set; }
        [Display(Name = "Opis filmu:")]
        public string Description { get; set; }
        [Display(Name = "Czas trwania filmu (minuty):")]
        [Range(1, 500)]
        public int Time { get; set;}
        [Display(Name = "Link do zdjęcia:")]
        public string PictureLink { get; set; }
    }
}
