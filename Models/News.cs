using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models
{
    public class News
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł:")]
        public string Title { get; set; }
        [Display(Name = "Opis:")]
        public string Description { get; set; }
        [Display(Name = "Data:")]
        public DateTime Date { get; set; }

    }
}
