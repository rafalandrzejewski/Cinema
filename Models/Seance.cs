namespace Cinema.Models
{
    public class Seance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MaxSeatCount { get; set; }
        public int FreeSeatCount { get; set;}
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
