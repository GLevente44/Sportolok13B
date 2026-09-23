namespace Sportolok13B.Models.DTOs
{
    public class EredmenyGet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? Password { get; set; }
        public DateTime RegTime { get; set; }
    }
}
