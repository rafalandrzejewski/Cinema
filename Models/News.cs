using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models
{
    public class News
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [Display(Name = "Tytuł:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany.")]
        [StringLength(100)]
        [Display(Name = "Opis:")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Data jest wymagana.")]
        [Display(Name = "Data:")]
        public DateTime Date { get; set; }

    }
}
